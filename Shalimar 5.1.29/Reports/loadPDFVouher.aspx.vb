Public Partial Class loadPDFVouher
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        Dim fileName As String

        fileName = Server.MapPath("..\TempDocument\") + Request.QueryString("TSID") + ".pdf"

        Response.ContentType = "Application/pdf"
        Response.TransmitFile(fileName)


    End Sub

End Class