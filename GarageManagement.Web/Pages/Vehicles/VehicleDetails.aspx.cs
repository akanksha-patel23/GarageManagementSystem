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
        private const int MaxPhotoBytes = 5 * 1024 * 1024; // 5 MB — matches 04-webforms-conventions.md
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
                BindMakeDropDown();
                BindVehicleTypeDropDown();

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

                    ddlMake.SelectedValue = vehicle.MakeID.ToString();
                    BindModelDropDown(vehicle.MakeID);
                    ddlModel.SelectedValue = vehicle.ModelID.ToString();

                    txtYear.Text = vehicle.Year?.ToString();
                    ddlVehicleType.SelectedValue = vehicle.VehicleTypeID?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(vehicle.PhotoPath))
                    {
                        lblExistingPhoto.Text = "Current photo on file — upload a new one only to replace it.";
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

        private void BindMakeDropDown()
        {
            ddlMake.DataSource = _vehicleBal.GetAllMakes();
            ddlMake.DataBind();
        }

        private void BindVehicleTypeDropDown()
        {
            ddlVehicleType.DataSource = _vehicleBal.GetAllVehicleTypes();
            ddlVehicleType.DataBind();
        }

        private void BindModelDropDown(int makeId)
        {
            ddlModel.DataSource = _vehicleBal.GetModelsByMake(makeId);
            ddlModel.DataBind();
        }

        protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Model list depends on the selected Make — reset it whenever Make changes.
            if (int.TryParse(ddlMake.SelectedValue, out int makeId))
            {
                BindModelDropDown(makeId);
            }
            else
            {
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("-- Select Model --", ""));
            }
        }

        protected void cvPhoto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // No file selected is valid — photo is optional, and on Edit the
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
                    MakeID = Convert.ToInt32(ddlMake.SelectedValue),
                    ModelID = Convert.ToInt32(ddlModel.SelectedValue),
                    Year = string.IsNullOrWhiteSpace(txtYear.Text) ? (int?)null : Convert.ToInt32(txtYear.Text),
                    VehicleTypeID = string.IsNullOrEmpty(ddlVehicleType.SelectedValue) ? (int?)null : Convert.ToInt32(ddlVehicleType.SelectedValue),
                    PhotoPath = photoPath // null on Edit means "keep existing" — handled in spUpdateVehicle
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