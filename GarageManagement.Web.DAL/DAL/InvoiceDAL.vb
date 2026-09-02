' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class InvoiceDAL

    Public Sub InsertInvoice(jobCardId As Integer, totalAmount As Decimal, paymentMode As String, ByRef invoiceId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertInvoice", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@JobCardID", SqlDbType.Int).Value = jobCardId
                    cmd.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = totalAmount
                    cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = CObj(If(paymentMode, DBNull.Value))

                    Dim invoiceIdParam As SqlParameter = cmd.Parameters.Add("@InvoiceID", SqlDbType.Int)
                    invoiceIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd)

                    invoiceId = Convert.ToInt32(invoiceIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdatePaymentStatus(invoiceId As Integer, paymentStatus As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateInvoicePaymentStatus", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId
                    cmd.Parameters.Add("@PaymentStatus", SqlDbType.NVarChar, 20).Value = paymentStatus

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

    Public Function GetAllInvoices() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllInvoices", conn)
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

    Public Function GetAllJobCardsForDropdown() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllJobCards", conn)
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