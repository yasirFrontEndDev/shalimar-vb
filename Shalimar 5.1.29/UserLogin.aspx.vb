Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports System.Configuration
Imports System.Management
Imports System.Net.NetworkInformation
Imports System.Reflection
Imports FMovers.Ticketing.Online

Partial Public Class UserLogin

    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim versionNumber As Version

        versionNumber = Assembly.GetExecutingAssembly().GetName().Version
        'Label1.Text = "Your configuration code is " & getMotherBoardID() & " .Please contact IT team to configure your settings."
        divFilesVer.InnerText = "Version " & versionNumber.ToString() & " Your Computer Name is " & Environment.MachineName

        objConnection = ConnectionManager.GetConnection()
        objUser = New clsUser(objConnection)
        lblCompany.Text = "You are going to login in  [ " & objUser.getCompanyName() & " ]"

        Me.RegisterClientScripts()
        txtLoginName.Focus()

        lblError.Style.Add("DISPLAY", "none")

    End Sub



    Public Function getMotherBoardID() As [String]
        Dim serial As [String] = ""
        Try
            Dim mos As New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard")
            Dim moc As ManagementObjectCollection = mos.[Get]()

            For Each mo As ManagementObject In moc
                serial = mo("SerialNumber").ToString()
            Next
            Return serial
        Catch generatedExceptionName As Exception
            Return serial
        End Try
    End Function



    Public Shared Function GetMacAddress() As String

        Dim ActiveMAC As String = ""
        For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()
            ' Only consider Ethernet network interfaces
            If nic.NetworkInterfaceType = NetworkInterfaceType.Ethernet AndAlso nic.OperationalStatus = OperationalStatus.Up Then
                ActiveMAC = nic.GetPhysicalAddress.ToString()
            End If
            If ActiveMAC = "" Then
                If nic.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 AndAlso nic.OperationalStatus = OperationalStatus.Up Then
                    ActiveMAC = nic.GetPhysicalAddress.ToString()
                End If
            End If
        Next
        Return ActiveMAC

    End Function


    Public Shared Function GetMACAddress2() As String
        Dim nics As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        Dim sMacAddress As [String] = String.Empty
        For Each adapter As NetworkInterface In nics
            If sMacAddress = [String].Empty Then
                ' only return MAC Address from first card
                'IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                sMacAddress = adapter.GetPhysicalAddress().ToString()
            End If
        Next
        Return sMacAddress
    End Function

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub RegisterClientScripts()
        Me.btnLogin.Attributes.Add("onclick", "return ValidateCredentials();")
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Try


            Dim Result As eAuthenticationResult
            objUser.LoginName = txtLoginName.Text.Trim()
            objUser.Password = txtPwd.Text


            Result = objUser.Login(txtLoginName.Text.Trim(), txtPwd.Text, getMotherBoardID)


            If Result = eAuthenticationResult.InvalidUserName Then
                lblError.Style.Add("DISPLAY", "")
                lblError.Text = "Invalid User Name."
                Return
            ElseIf Result = eAuthenticationResult.InvalidPassword Then
                lblError.Style.Add("DISPLAY", "")
                lblError.Text = "Invalid Password."
                Return
            ElseIf Result = eAuthenticationResult.UserInActive Then
                lblError.Style.Add("DISPLAY", "")
                lblError.Text = "User is not Active. Please contact system Administrator."
                Return
            End If

            Session("CurrentUser") = objUser
            Dim objOnlineTicketing As eTicketing
            objOnlineTicketing = New eTicketing

            If ServerPing() Then
                Dim versionNumber As Version
                versionNumber = Assembly.GetExecutingAssembly().GetName().Version

                objOnlineTicketing.AddUpdatedComputer(Environment.MachineName, objUser.TerminalId, 0, versionNumber.ToString())
            End If

            '    Response.Redirect("main.aspx")
            Response.Redirect("main.aspx")


        Catch ex As Exception
            lblError.Text = ex.Message

        End Try

    End Sub

    Private Function ServerPing() As Boolean

        Try
            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))



            If pingreply.Status = IPStatus.Success Then
                Return True
            Else

                Return False
            End If

        Catch ex As Exception
            Return False

        End Try

    End Function


End Class