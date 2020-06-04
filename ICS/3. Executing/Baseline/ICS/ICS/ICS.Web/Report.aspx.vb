Imports System.IO


Partial Public Class Report
    Inherits System.Web.UI.Page


#Region "Methods"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strDispType As String

        gvExcel.Visible = False

        'Determine if this is a crystal report or an Excel document
        strDispType = Request("DispType")
        If strDispType = "R" Then
            Try
                Dim IOStream As Stream = Global_asax.IOStream
                Response.Clear()

                'set type of file to be streamed and set filename when streamed out
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "filename=" & "Report.pdf")

                'stream file to the client
                Dim ByteStream(CType(IOStream.Length, Integer)) As Byte
                IOStream.Read(ByteStream, 0, CInt(IOStream.Length))
                Response.BinaryWrite(ByteStream)

                'Set cache setting to a small number of seconds so that the page_load event is not fired twice on initial hit
                Response.Cache.SetMaxAge(New TimeSpan(0, 0, 5))

                Response.End()
            Catch ex As Exception
                Response.Write(ex.StackTrace)
            End Try
        ElseIf strDispType = "E" Then ' Excel

            gvExcel.DataSource = Session("reportselectorview_aspxDataSet").tables(2)
            gvExcel.DataBind()


            gvExcel.Visible = True
            Dim frm As HtmlForm = New HtmlForm()
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            ' Remove the charset from the Content-Type header.
            Response.Charset = ""
            Response.AddHeader("content-disposition", "attachment;filename=CCCProductDescription.xls")
            Me.EnableViewState = False
            Dim tw As New System.IO.StringWriter
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Controls.Add(frm)
            frm.Controls.Add(gvExcel)
            frm.RenderControl(hw)
            frm.Visible = True

            Response.Write(tw.ToString())
            ' End the response.
            Response.End()

        End If
    End Sub



#End Region


End Class