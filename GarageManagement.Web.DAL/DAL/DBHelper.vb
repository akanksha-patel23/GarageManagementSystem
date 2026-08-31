
'Imports System.Configuration
'Imports System.Data.SqlClient

'Namespace GarageManagement.Web.DAL
'    Public NotInheritable Class DBHelper
'        Private Sub New()
'        End Sub
'    End Class
'End Namespace


' Project: GarageManagement.Web.DAL (root namespace set to GarageManagement.Web.DAL
' in project properties, so no explicit Namespace block needed here)

'Imports Microsoft.Data.SqlClient
Imports System.Configuration
Imports System
Imports System.Data.SqlClient

Public Class DBHelper

    Public Shared Function GetConnection() As SqlConnection
        Dim connString As String = ConfigurationManager.ConnectionStrings("GarageManagementDB").ConnectionString
        Return New SqlConnection(connString)
    End Function

    ''' <summary>
    ''' Per 03-error-handling.md: every write/read SP returns @ErrorID OUTPUT.
    ''' Call this only after the SP's output params are guaranteed available —
    ''' i.e. after ExecuteNonQuery completes, or after a SqlDataReader has been
    ''' fully closed, or after SqlDataAdapter.Fill has completed. Throws if the
    ''' SP logged a failure via GLOG.spLogDBErrorDetail.
    ''' </summary>
    Public Shared Sub CheckErrorId(cmd As SqlCommand)
        Dim param As SqlParameter = cmd.Parameters("@ErrorID")
        If param IsNot Nothing AndAlso param.Value IsNot Nothing AndAlso param.Value IsNot DBNull.Value Then
            Dim errorId As Integer = Convert.ToInt32(param.Value)
            If errorId <> 0 Then
                Throw New ApplicationException($"Database operation failed. Reference ID: {errorId}")
            End If
        End If
    End Sub

End Class