'' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

'Imports System
'Imports System.Data
'Imports System.Data.SqlClient

'''' <summary>
'''' Data Access Layer for JobCards. Raw ADO.NET only. Returns DataTable - never
'''' the JobCard entity - per 01-layering-and-architecture.md's DAL-return-type rule.
'''' </summary>
'Public Class JobCardDAL

'    Public Function GetAllJobCards() As DataTable
'        Dim dt As New DataTable()

'        Try
'            Using conn As SqlConnection = DBHelper.GetConnection()
'                Using cmd As New SqlCommand("spGetAllJobCards", conn)
'                    cmd.CommandType = CommandType.StoredProcedure

'                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
'                    errorIdParam.Direction = ParameterDirection.Output

'                    Using adapter As New SqlDataAdapter(cmd)
'                        adapter.Fill(dt)
'                    End Using

'                    DBHelper.CheckErrorId(cmd)
'                End Using
'            End Using
'        Catch ex As SqlException
'            Throw
'        End Try

'        Return dt
'    End Function

'End Class

' Project: GarageManagement.Web.DAL (root namespace = GarageManagement.Web.DAL)

Imports System
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Data Access Layer for JobCards — listing, viewing, and status updates ONLY.
''' Per 01-layering-and-architecture.md / Flow B in 10-canonical-flows.md,
''' JobCard CREATION happens exclusively through the API's JobCardRepository
''' (SqlTransaction across header + lines + stock decrement) — this class
''' never inserts a JobCard.
''' </summary>
Public Class JobCardDAL

    Public Function GetAllJobCards() As DataTable
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

    Public Function GetJobCardById(jobCardId As Integer) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetJobCardById", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@JobCardID", SqlDbType.Int).Value = jobCardId

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        dt.Load(reader)
                    End Using ' reader closed before checking @ErrorID

                    DBHelper.CheckErrorId(cmd)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try

        Return dt
    End Function

    ''' <summary>
    ''' spUpdateJobCardStatus writes both the status change and its history
    ''' entry atomically inside the SP itself — no separate history insert here.
    ''' </summary>
    Public Sub UpdateJobCardStatus(jobCardId As Integer, newStatus As String, remarks As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateJobCardStatus", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@JobCardID", SqlDbType.Int).Value = jobCardId
                    cmd.Parameters.Add("@NewStatus", SqlDbType.NVarChar, 20).Value = newStatus
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = CObj(If(remarks, DBNull.Value))

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
    ''' Used by JobCardStatusBoard.aspx's UpdatePanel+Timer live refresh (Flow E) —
    ''' the same SP the API exposes at GET /api/jobcards/{id}/status-history for
    ''' external consumers, but called here directly via BAL/DAL for the board itself.
    ''' </summary>
    Public Function GetStatusHistoryByJobCard(jobCardId As Integer) As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetStatusHistoryByJobCard", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@JobCardID", SqlDbType.Int).Value = jobCardId

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