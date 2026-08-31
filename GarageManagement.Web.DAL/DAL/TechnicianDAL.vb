'Namespace GarageManagement.Web.DAL
'    Public Class TechnicianDAL
'    End Class
'End Namespace

' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
'Imports Microsoft.Data.SqlClient
Imports System.Data.SqlClient

Public Class TechnicianDAL

    Public Sub InsertTechnician(name As String, specialization As String, contactNumber As String, ByRef technicianId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertTechnician", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name
                    cmd.Parameters.Add("@Specialization", SqlDbType.NVarChar, 100).Value = CObj(If(specialization, DBNull.Value))
                    cmd.Parameters.Add("@ContactNumber", SqlDbType.NVarChar, 20).Value = CObj(If(contactNumber, DBNull.Value))

                    Dim technicianIdParam As SqlParameter = cmd.Parameters.Add("@TechnicianID", SqlDbType.Int)
                    technicianIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd)

                    technicianId = Convert.ToInt32(technicianIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdateTechnician(technicianId As Integer, name As String, specialization As String, contactNumber As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateTechnician", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@TechnicianID", SqlDbType.Int).Value = technicianId
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name
                    cmd.Parameters.Add("@Specialization", SqlDbType.NVarChar, 100).Value = CObj(If(specialization, DBNull.Value))
                    cmd.Parameters.Add("@ContactNumber", SqlDbType.NVarChar, 20).Value = CObj(If(contactNumber, DBNull.Value))

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

    Public Sub DeleteTechnician(technicianId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeleteTechnician", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@TechnicianID", SqlDbType.Int).Value = technicianId

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

    Public Function GetAllTechnicians() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllTechnicians", conn)
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