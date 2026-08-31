using GarageManagement.Web.BAL;
using GarageManagement.Web.Entities;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarageManagement.Web.Pages.Vehicles
{
    public partial class VehicleDetails : System.Web.UI.Page
    {
        private readonly VehicleBAL _vehicleBal = new VehicleBAL();
        private readonly CustomerBAL _customerBal = new CustomerBAL();

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const int MaxPhotoBytes = 5 * 1024 * 1024; // 5 MB � matches 04-webforms-conventions.md
        private const string UploadFolder = "~/Uploads/VehiclePhotos/";

        private int? VehicleId
        {
            get
            {
                if (int.TryParse(Request.QueryString["VehicleID"], out int id))
                    return id;
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerDropDown();

                if (VehicleId.HasValue)
                {
                    var vehicle = _vehicleBal.GetVehicleById(VehicleId.Value);
                    if (vehicle == null)
                    {
                        ShowMessage("Vehicle not found.", isError: true);
                        return;
                    }

                    ddlCustomer.SelectedValue = vehicle.CustomerID.ToString();
                    txtRegNumber.Text = vehicle.RegNumber;
                    txtMake.Text = vehicle.Make;
                    txtModel.Text = vehicle.Model;
                    txtYear.Text = vehicle.Year?.ToString();
                    ddlVehicleType.SelectedValue = vehicle.VehicleType ?? "Other";

                    if (!string.IsNullOrEmpty(vehicle.PhotoPath))
                    {
                        lblExistingPhoto.Text = "Current photo on file � upload a new one only to replace it.";
                        lblExistingPhoto.Visible = true;
                    }
                }
            }
        }

        private void BindCustomerDropDown()
        {
            ddlCustomer.DataSource = _customerBal.GetAllCustomers();
            ddlCustomer.DataBind();
        }

        protected void cvPhoto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // No file selected is valid � photo is optional, and on Edit the
            // existing PhotoPath is preserved (see spUpdateVehicle's COALESCE).
            if (!fuPhoto.HasFile)
            {
                args.IsValid = true;
                return;
            }

            string extension = Path.GetExtension(fuPhoto.FileName).ToLowerInvariant();
            bool validExtension = Array.IndexOf(AllowedExtensions, extension) >= 0;
            bool validSize = fuPhoto.PostedFile.ContentLength <= MaxPhotoBytes;

            args.IsValid = validExtension && validSize;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string photoPath = null;
                if (fuPhoto.HasFile)
                {
                    photoPath = SavePhoto(fuPhoto);
                }

                var vehicle = new Vehicle
                {
                    VehicleID = VehicleId ?? 0,
                    CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue),
                    RegNumber = txtRegNumber.Text.Trim(),
                    Make = txtMake.Text.Trim(),
                    Model = txtModel.Text.Trim(),
                    Year = string.IsNullOrWhiteSpace(txtYear.Text) ? (int?)null : Convert.ToInt32(txtYear.Text),
                    VehicleType = ddlVehicleType.SelectedValue,
                    PhotoPath = photoPath // null on Edit means "keep existing" � handled in spUpdateVehicle
                };

                if (VehicleId.HasValue)
                    _vehicleBal.UpdateVehicle(vehicle);
                else
                    _vehicleBal.InsertVehicle(vehicle);

                Response.Redirect("~/Pages/Vehicles/VehicleList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Unable to save vehicle. " + ex.Message, isError: true);
            }
        }

        private string SavePhoto(FileUpload upload)
        {
            string uploadDir = Server.MapPath(UploadFolder);
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(upload.FileName)}";
            string fullPath = Path.Combine(uploadDir, fileName);
            upload.SaveAs(fullPath);

            return UploadFolder.TrimStart('~') + fileName;
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isError ? "msc-message msc-message-error" : "msc-message msc-message-success";
            lblMessage.Visible = true;
        }
    }
}
