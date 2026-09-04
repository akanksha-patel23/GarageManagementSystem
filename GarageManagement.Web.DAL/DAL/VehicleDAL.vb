'Namespace GarageManagement.Web.DAL
'    Public Class VehicleDAL
'    End Class
'End Namespace

' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
Imports System.Data.SqlClient
'Imports Microsoft.Data.SqlClient

''' <summary>
''' Data Access Layer for Vehicles. Raw ADO.NET only. Returns DataTable — never
''' the Vehicle entity — per 01-layering-and-architecture.md's DAL-return-type rule.
''' </summary>
Public Class VehicleDAL

    Public Sub InsertVehicle(customerId As Integer, regNumber As String, makeId As Integer, modelId As Integer,
                              year As Integer?, vehicleTypeId As Integer?, photoPath As String, ByRef vehicleId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertVehicle", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId
                    cmd.Parameters.Add("@RegNumber", SqlDbType.NVarChar, 20).Value = regNumber
                    cmd.Parameters.Add("@MakeID", SqlDbType.Int).Value = makeId
                    cmd.Parameters.Add("@ModelID", SqlDbType.Int).Value = modelId
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = CObj(If(year.HasValue, CType(year.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("@VehicleTypeID", SqlDbType.Int).Value = CObj(If(vehicleTypeId.HasValue, CType(vehicleTypeId.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("@PhotoPath", SqlDbType.NVarChar, 500).Value = CObj(If(photoPath, DBNull.Value))

                    Dim vehicleIdParam As SqlParameter = cmd.Parameters.Add("@VehicleID", SqlDbType.Int)
                    vehicleIdParam.Direction = ParameterDirection.Output
                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    DBHelper.CheckErrorId(cmd)
                    vehicleId = Convert.ToInt32(vehicleIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdateVehicle(vehicleId As Integer, regNumber As String, makeId As Integer, modelId As Integer,
                              year As Integer?, vehicleTypeId As Integer?, photoPath As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateVehicle", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value = vehicleId
                    cmd.Parameters.Add("@RegNumber", SqlDbType.NVarChar, 20).Value = regNumber
                    cmd.Parameters.Add("@MakeID", SqlDbType.Int).Value = makeId
                    cmd.Parameters.Add("@ModelID", SqlDbType.Int).Value = modelId
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = CObj(If(year.HasValue, CType(year.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("@VehicleTypeID", SqlDbType.Int).Value = CObj(If(vehicleTypeId.HasValue, CType(vehicleTypeId.Value, Object), DBNull.Value))
                    ' Null means "keep the existing photo" — spUpdateVehicle uses COALESCE.
                    cmd.Parameters.Add("@PhotoPath", SqlDbType.NVarChar, 500).Value = CObj(If(photoPath, DBNull.Value))

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub DeleteVehicle(vehicleId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeleteVehicle", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value = vehicleId

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Function GetVehicleById(vehicleId As Integer) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetVehicleById", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value = vehicleId

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
        Return dt
    End Function

    Public Function GetAllVehicles() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllVehicles", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
        Return dt
    End Function

    ' ---- Lookup reads for the Make/Model/VehicleType dropdowns ----

    Public Function GetAllMakes() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllMakes", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
        Return dt
    End Function

    Public Function GetModelsByMake(makeId As Integer) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetModelsByMake", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@MakeID", SqlDbType.Int).Value = makeId
                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
        Return dt
    End Function

    Public Function GetAllVehicleTypes() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllVehicleTypes", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
        Return dt
    End Function

End Class