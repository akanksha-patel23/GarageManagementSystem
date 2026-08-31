Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class ServiceCategoryDAL
    Public Sub InsertServiceCategory(parentCategoryId As Integer?, categoryName As String, ByRef categoryId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spInsertServiceCategory", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@ParentCategoryID", SqlDbType.Int).Value = CObj(If(parentCategoryId.HasValue, CType(parentCategoryId.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = categoryName

                    Dim categoryIdParam As SqlParameter = cmd.Parameters.Add("@CategoryID", SqlDbType.Int)
                    categoryIdParam.Direction = ParameterDirection.Output

                    Dim errorIdParam As SqlParameter = cmd.Parameters.Add("@ErrorID", SqlDbType.Int)
                    errorIdParam.Direction = ParameterDirection.Output

                    conn.Open()
                    cmd.ExecuteNonQuery()

                    DBHelper.CheckErrorId(cmd)

                    categoryId = Convert.ToInt32(categoryIdParam.Value)
                End Using
            End Using
        Catch ex As SqlException
            Throw
        End Try
    End Sub

    Public Sub UpdateServiceCategory(categoryId As Integer, parentCategoryId As Integer?, categoryName As String)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spUpdateServiceCategory", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId
                    cmd.Parameters.Add("@ParentCategoryID", SqlDbType.Int).Value = CObj(If(parentCategoryId.HasValue, CType(parentCategoryId.Value, Object), DBNull.Value))
                    cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = categoryName

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

    Public Sub DeleteServiceCategory(categoryId As Integer)
        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spDeleteServiceCategory", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = categoryId

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
    ''' SqlDataAdapter/DataTable pattern. Returns the full hierarchy (with a
    ''' Depth column) — used both for the admin TreeView and, indented by Depth,
    ''' for the flat category DropDownList inside JobCardWizard.aspx.
    ''' </summary>
    Public Function GetServiceCategoryTree() As DataTable
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = DBHelper.GetConnection()
                Using cmd As New SqlCommand("spGetServiceCategoryTree", conn)
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
