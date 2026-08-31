'Namespace GarageManagement.Web.DAL
'    Public Class ServiceCatalogDAL
'    End Class
'End Namespace

' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
'Imports Microsoft.Data.SqlClient
Imports System.Data.SqlClient

''' <summary>
''' Data Access Layer for ServiceCategories. Raw ADO.NET only. Returns DataTable —
''' never the ServiceCategory entity — per 01-layering-and-architecture.md.
''' </summary>
Public Class ServiceCatalogDAL

    Public Sub InsertServiceCatalog(categoryId As Integer, serviceName As String, basePrice As Decimal,
                                     estimatedHours As Decimal?, ByRef serviceId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertServiceCatalog", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId
                    cmd.Parameters.Add("@ServiceName", SqlDbType.NVarChar, 100).Value = serviceName
                    cmd.Parameters.Add("@BasePrice", SqlDbType.Decimal).Value = basePrice
                    cmd.Parameters.Add("@EstimatedHours", SqlDbType.Decimal).Value = CObj(If(estimatedHours.HasValue, CType(estimatedHours.Value, Object), DBNull.Value))

                    Dim serviceIdParam As SqlParameter = cmd.Parameters.Add("@ServiceID", SqlDbType.Int)
                    serviceIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd)

                    serviceId = Convert.ToInt32(serviceIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdateServiceCatalog(serviceId As Integer, categoryId As Integer, serviceName As String,
                                     basePrice As Decimal, estimatedHours As Decimal?)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateServiceCatalog", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = serviceId
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId
                    cmd.Parameters.Add("@ServiceName", SqlDbType.NVarChar, 100).Value = serviceName
                    cmd.Parameters.Add("@BasePrice", SqlDbType.Decimal).Value = basePrice
                    cmd.Parameters.Add("@EstimatedHours", SqlDbType.Decimal).Value = CObj(If(estimatedHours.HasValue, CType(estimatedHours.Value, Object), DBNull.Value))

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

    Public Sub DeleteServiceCatalog(serviceId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeleteServiceCatalog", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@ServiceID", SqlDbType.Int).Value = serviceId

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

    Public Function GetAllServiceCatalog() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllServiceCatalog", conn)
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

