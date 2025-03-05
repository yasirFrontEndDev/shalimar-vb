<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head>
<title> </title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body onload="window.print(); window.close();">

<% 

response.write("Ticket Sr. No. :" & Request.Querystring("TicketNo"))
response.write("<BR>")
response.write("Passenger Name :" & Request.Querystring("PassengerName"))
response.write("<BR>")

response.write("Contact No. :" & Request.Querystring("ContractNo"))
response.write("<BR>")
response.write("Seat No. :" & Request.Querystring("SeatNo"))
response.write("<BR>")
response.write("Fare :" & Request.Querystring("Fare"))
response.write("<BR>")
response.write("Route :" & Request.Querystring("Route"))
response.write("<BR>")
response.write("Time :" & Request.Querystring("DepartureDateTime"))
response.write("<BR>")
response.write("Bus Reg :" & Request.Querystring("VehicleNo"))


%>
<script language='VBScript'> 
Sub Print() 
       OLECMDID_PRINT = 6 
       OLECMDEXECOPT_DONTPROMPTUSER = 2 
       OLECMDEXECOPT_PROMPTUSER = 1 
       call WB.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER,1) 
End Sub 
document.write "<object ID='WB' WIDTH=0 HEIGHT=0 CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'></object>" 
</script> 

</body>
</html>
