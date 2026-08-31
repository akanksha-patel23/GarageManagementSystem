Imports System
Imports System.Data
Imports System.Data.SqlClient
'Imports Microsoft.Data.SqlClient


'Namespace GarageManagement.Web.DAL
'    Public Class CustomerDAL
'    End Class
'End Namespace

' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)


''' <summary>
''' Data Access Layer for Customers. Raw ADO.NET only � no business rules, no
''' entity mapping (returns DataTable to avoid a circular reference with the
''' GarageManagement.Web project, which owns the Customer entity).
'''
''' GetCustomerById demonstrates SqlConnection/SqlCommand/SqlDataReader.
''' GetAllCustomers demonstrates SqlDataAdapter/DataTable � both ADO.NET
''' techniques are represented here per the Day 3 plan requirement.
''' </summary>
Public Class CustomerDAL

    Public Sub InsertCustomer(name As String, phone As String, email As String,
                               address As String, ByRef customerId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertCustomer", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = CObj(If(email, DBNull.Value))
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = CObj(If(address, DBNull.Value))

                    Dim customerIdParam As SqlParameter = cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                    customerIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd) ' throws if @ErrorID <> 0

                    customerId = Convert.ToInt32(customerIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            ' Connection/timeout-level failures happen before the SP's own CATCH can
            ' log to GLOG � no client-side sink is defined for these yet. Rethrow so
            ' BAL/UI surfaces a generic error while ex.Message stays for debugging.
            Throw
        End Try
    End Sub

    Public Sub UpdateCustomer(customerId As Integer, name As String, phone As String,
                               email As String, address As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateCustomer", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = CObj(If(email, DBNull.Value))
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = CObj(If(address, DBNull.Value))

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

    Public Sub DeleteCustomer(customerId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeleteCustomer", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId

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

    ''' <summary>
    ''' Fundamentals demo: raw SqlConnection/SqlCommand/SqlDataReader. The reader
    ''' is closed (Using block ends) before @ErrorID is read � required per
    ''' 03-error-handling.md, since output params only populate after the reader closes.
    ''' </summary>
    Public Function GetCustomerById(customerId As Integer) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetCustomerById", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using ' reader closed here � safe to check @ErrorID now

                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try

        Return dt
    End Function

    ''' <summary>
    ''' SqlDataAdapter/DataTable demo (Day 3 requirement). Adapter.Fill implicitly
    ''' handles open/execute/close, and populates output params correctly afterward.
    ''' </summary>
    Public Function GetAllCustomers() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllCustomers", conn)
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
            ' Log full exception details for debugging
            System.Diagnostics.EventLog.WriteEntry("Application",
                $"GetAllCustomers DAL failed: {ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}",
                System.Diagnostics.EventLogEntryType.Error)
            Throw
        End Try

        Return dt
    End Function

    Public Function SearchCustomers(searchTerm As String) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spSearchCustomers", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@SearchTerm", SqlDbType.NVarChar, 100).Value = searchTerm

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