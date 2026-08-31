' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Data Access Layer for Parts. Raw ADO.NET only. Returns DataTable — never
''' the Part entity — per 01-layering-and-architecture.md.
''' spUpdatePart does not touch StockQty (spUpdatePartStock owns that, keeping
''' "edit part details" and "adjust stock" as separate, explicit actions).
''' </summary>
Public Class PartDAL

    Public Sub InsertPart(partName As String, stockQty As Integer, unitPrice As Decimal, ByRef partId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertPart", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@PartName", SqlDbType.NVarChar, 100).Value = partName
                    cmd.Parameters.Add("@StockQty", SqlDbType.Int).Value = stockQty
                    cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = unitPrice

                    Dim partIdParam As SqlParameter = cmd.Parameters.Add("@PartID", SqlDbType.Int)
                    partIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd)

                    partId = Convert.ToInt32(partIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdatePart(partId As Integer, partName As String, unitPrice As Decimal)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdatePart", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@PartID", SqlDbType.Int).Value = partId
                    cmd.Parameters.Add("@PartName", SqlDbType.NVarChar, 100).Value = partName
                    cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = unitPrice

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

    Public Sub DeletePart(partId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeletePart", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@PartID", SqlDbType.Int).Value = partId

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

    Public Function GetAllParts() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllParts", conn)
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

    ''' <summary>
    ''' Wraps spUpdatePartStock. qtyChange negative decrements, positive increments —
    ''' same SP the JobCard creation transaction uses on the API side.
    ''' </summary>
    Public Sub UpdateStock(partId As Integer, qtyChange As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdatePartStock", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@PartID", SqlDbType.Int).Value = partId
                    cmd.Parameters.Add("@QtyChange", SqlDbType.Int).Value = qtyChange

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

End Class