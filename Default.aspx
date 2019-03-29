<%@ Page Language="VB" AutoEventWireup="false" CodeFile="default.aspx.vb" Inherits="_Payments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Payment Portal</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td align="right" style="width: 213px" valign="top">
                    Amount</td>
                <td valign="top">
                </td>
                <td style="width: 224px" valign="top">
                    <asp:TextBox ID="txtAmount" runat="server" Width="106px"></asp:TextBox>
                    <asp:DropDownList ID="lstCurrency" runat="server">
                        <asp:ListItem Value="GBP"></asp:ListItem>
                        <asp:ListItem Value="EUR"></asp:ListItem>
                        
                    </asp:DropDownList></td>
                <td style="width: 340px" valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtAmount"
                        ErrorMessage='"Amount" missing.'></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right" style="width: 213px; height: 20px" valign="top">
                    Card Holder Name</td>
                <td style="width: 10px; height: 20px" valign="top">
                </td>
                <td style="width: 224px; height: 20px" valign="top">
                    <asp:TextBox ID="txtCardHolder" runat="server" Width="200px"></asp:TextBox></td>
                <td style="width: 340px; height: 20px" valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAmount"
                        ErrorMessage='"Card Holder Name" missing.'></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right" style="width: 213px; height: 24px;" valign="top">
                    Card Type</td>
                <td style="height: 24px" valign="top">
                </td>
                <td style="width: 224px; height: 24px;" valign="top">
                    <asp:DropDownList ID="lstCardType" runat="server">
                        <asp:ListItem Value="VISA" Text="Visa/Delta"></asp:ListItem>
                        <asp:ListItem Value="MC" Text="Mastercard"></asp:ListItem>
                        <asp:ListItem Value="SWITCH" Text="Switch/Solo"></asp:ListItem>
                        <asp:ListItem Value="AMEX" Text="American Express"></asp:ListItem>
                        <asp:ListItem Value="LASER" Text="Laser"></asp:ListItem>
                        <asp:ListItem Value="DINERS" Text="Diners"></asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 340px; height: 24px" valign="top">
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 213px" valign="top">
                    Card Number</td>
                <td valign="top">
                </td>
                <td style="width: 224px" valign="top">
                    <asp:TextBox ID="txtCardNum" runat="server" Width="200px"></asp:TextBox>
                    <asp:Label ID="txt_CardNumHide" runat="server" Visible="False"></asp:Label></td>
                <td style="width: 340px" valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCardNum"
                        ErrorMessage='"Card Number" missing.'></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="right" style="width: 213px" valign="top">
                    Card Expiry</td>
                <td valign="top">
                </td>
                <td style="width: 224px" valign="top">
                    <asp:DropDownList ID="lstMonth" runat="server">

                        
                    </asp:DropDownList>
                    <asp:DropDownList ID="lstYear" runat="server">
                    </asp:DropDownList></td>
                <td style="width: 340px" valign="top">
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 213px" valign="top">
                    CVN</td>
                <td valign="top">
                </td>
                <td style="width: 224px" valign="top">
                    <asp:TextBox ID="txtCVN" runat="server" Width="60px"></asp:TextBox></td>
                <td style="width: 340px" valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCVN"
                        ErrorMessage='"CVN" missing.'></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCVN"
                        ErrorMessage='Invalid "CVN"' ValidationExpression="\d{3}"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td align="right" style="width: 213px; height: 20px;" valign="top">
                </td>
                <td style="height: 20px" valign="top">
                </td>
                <td style="width: 224px; height: 20px;" valign="top">
                    <asp:Button ID="btn" runat="server" Text="Process Payment" /></td>
                <td style="width: 340px; height: 20px" valign="top">
                    <asp:Button ID="btnNewCard" runat="server" Text="New Card" Visible="False" /></td>
            </tr>
            <tr>
                <td align="right" style="width: 213px; height: 21px" valign="top">
                </td>
                <td style="height: 21px" valign="top">
                </td>
                <td colspan="2" style="height: 21px" valign="top">
                    <asp:Label ID="txtResult" runat="server" Width="280px"></asp:Label><br />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                </td>
            </tr>
        </table>
    
    </div>
        <asp:HiddenField ID="ENV" runat="server" Visible="False" />
    </form>
</body>
</html>
