
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Imports GsmComm.PduConverter
Imports GsmComm.GsmCommunication
Imports System.Drawing.Printing

Imports System.Management


Partial Public Class UserCashClosing
    Inherits System.Web.UI.Page

    Dim objBusCharges As clsBusCharges
    Dim objConnection As Object
    Dim objUser As clsUser
    Public UserName As String
    Public CurrentUserID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        Else
            objUser = CType(Session("CurrentUser"), clsUser)
            CurrentUserID = "" & objUser.Id
            UserID.Value = "" & objUser.Id

            UserName = objUser.LoginName.ToLower()
        End If

        If Page.IsPostBack = False Then
            loadCombes()
            loadbalance()
        End If


    End Sub

    Private Sub loadbalance()



        Dim strClosing As String = ""
        Dim Arr As Array

        objConnection = ConnectionManager.GetConnection()

        objBusCharges = New clsBusCharges(objConnection)
        objBusCharges.UserId = CurrentUserID

        Dim Openning As Integer = objBusCharges.GetOpeningBalance(CurrentUserID)

        strClosing = objBusCharges.GetByTicketingAdvanceById()
        Arr = strClosing.Split("~")

        If Arr.Length > 1 Then

            lblAdance.Text = Arr(0)
            lblCashCollection.Text = Arr(1)
            lblDeduction.Text = Arr(2)
            lblTotal.Text = CDbl(Arr(1)) + CDbl(Arr(2))

        Else

            lblAdance.Text = "0"
            lblCashCollection.Text = "0"
            lblDeduction.Text = "0"
            lblTotal.Text = "0"

        End If

        lblOpeniningBal.Text = Openning.ToString()



    End Sub

    Private Sub loadCombes()

        Dim dtDataTable As New DataTable

        dtDataTable = objUser.GetAll()
        cboUsers.Items.Clear()
        cboUsers.DataSource = dtDataTable
        cboUsers.DataValueField = "User_Id"
        cboUsers.DataTextField = "Comeplete"
        cboUsers.DataBind()

        cboSchedule.Items.Clear()
        dtDataTable = objUser.getUserClosingSchedules()
        cboSchedule.Items.Clear()
        cboSchedule.DataSource = dtDataTable
        cboSchedule.DataValueField = "Schedule_ID"
        cboSchedule.DataTextField = "Schedule_Title"
        cboSchedule.DataBind()


    End Sub


    Protected Sub btnShowReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowReport.Click

        objConnection = ConnectionManager.GetConnection()

        objBusCharges = New clsBusCharges(objConnection)


        Dim Result As Integer = 0

        objUser.LoginName = txtLoginName.Text.Trim()
        objUser.Password = txtPwd.Text

        Result = objUser.LoginForClosing(txtLoginName.Text.Trim(), txtPwd.Text)

        If Result = 0 Or Result = CurrentUserID Then
            lblError.Style.Add("DISPLAY", "")
            lblError.Text = "Invalid User Name."
            Return

        End If


        Dim Total As Integer = 0, ShiftToCashier As Integer = 0

        If chkCashToCashier.Checked = True Then
            ShiftToCashier = 1
        End If


        Total = (CInt(lblOpeniningBal.Text)) + (CInt(lblCashCollection.Text)) + (CInt(lblAdance.Text))

        If (Total = 0) Then

            lblError.Style.Add("DISPLAY", "")
            lblError.Text = "User Closing can not be proceeded. Not traction to proceed."


        Else

            If CInt(Val(cboUsers.SelectedValue)) = "0" Then
                lblError.Style.Add("DISPLAY", "")
                lblError.Text = "User Closing can not be proceeded. Plese select user to shift cash."
            Else

                objBusCharges.UserCashClosingInsert(2, CurrentUserID, cboUsers.SelectedValue, Total, ShiftToCashier)
                btnShowReport.Visible = False
                lblOK.Visible = True

            End If



        End If

        loadbalance()


        ' Will uncommect at the end


    End Sub
End Class