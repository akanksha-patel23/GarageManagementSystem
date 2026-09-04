Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class UserDAL

    Public Sub InsertUser(username As String, passwordHash As String, passwordSalt As String,
                           fullName As String, role As String, ByRef userId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertUser", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = passwordHash
                    cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 255).Value = passwordSalt
                    cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = fullName
                    cmd.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = role

                    Dim userIdParam As SqlParameter = cmd.Parameters.Add("@NewUserID", SqlDbType.Int)
                    userIdParam.Direction = ParameterDirection.Output
                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()
                    DBHelper.CheckErrorId(cmd)
                    userId = Convert.ToInt32(userIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Function GetUserByUsername(username As String) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetUserByUsername", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username

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

    Public Function GetAllUsers() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetAllUsers", conn)
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

    Public Sub UpdateUserActive(userId As Integer, isActive As Boolean)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateUserActive", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive

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