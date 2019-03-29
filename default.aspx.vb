Imports System.Data
Imports System.Data.SqlClient
Imports PriPROC6.Interface.Web

Partial Class _Payments
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ref As String = Request.Params("REF")
        Dim iType As String = Request.Params("TYPE")
        Dim env As String = Request.Params("ENV")
        If IsNothing(ref) Then Throw New Exception("No Invoice reference")

        If Not Page.IsPostBack Then
            With Me
                For m As Integer = 1 To 12
                    .lstMonth.Items.Add(m.ToString)
                Next
                For y As Integer = Year(Now) To Year(Now) + 10
                    .lstYear.Items.Add(y.ToString)
                Next
            End With
        End If

        Dim result As New System.Data.DataTable
        Using cn As New SqlConnection(ConfigurationManager.ConnectionStrings("priority").ConnectionString)
            With cn
                .Open()
                .ChangeDatabase(env)
            End With

            Using cmd As New SqlCommand("", cn)
                With cmd
                    Select Case iType
                        Case "E"
                            .CommandText = Replace(ConfigurationManager.AppSettings.Get("_SQLQueryE"), "%qIVNUM%", ref)
                            With Me.btnNewCard
                                .Visible = False
                            End With

                        Case "T"
                            .CommandText = Replace(ConfigurationManager.AppSettings.Get("_SQLQueryT"), "%qIVNUM%", ref)

                        Case "SO"
                            .CommandText = Replace(ConfigurationManager.AppSettings.Get("_SQLQuerySO"), "%qIVNUM%", ref)

                    End Select
                    result.Load(.ExecuteReader())
                    With result
                        For Each row As DataRow In result.Rows
                            For Each col As DataColumn In result.Columns
                                Select Case col.ColumnName
                                    Case Else
                                        Dim o As Object = Page.FindControl(col.ColumnName)

                                        Try
                                            If Not IsNothing(o) Then
                                                o.Text = CStr(row(col.ColumnName))
                                            Else
                                                Dim fld As New HiddenField()
                                                fld.ID = col.ColumnName
                                                fld.Value = row(col.ColumnName)
                                                Page.Form.Controls.Add(fld)
                                            End If

                                        Catch ex As Exception

                                        End Try

                                End Select

                            Next
                        Next

                    End With

                End With

            End Using

        End Using

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn.Click

        Me.btn.Enabled = False

        Dim tReq As RealexPayments.RealAuth.TransactionRequest
        Dim ccReq As RealexPayments.RealAuth.CreditCard
        Dim tResp As RealexPayments.RealAuth.TransactionResponse

        Dim backgroundColour As Drawing.Color
        Dim displayMessage As String = String.Empty

        Dim MerchantName As String = Nothing
        Dim normalPassword As String = Nothing
        Dim rebatePassword As String = Nothing
        Dim refundPassword As String = Nothing
        Dim transAccount As String = Nothing

        With ConfigurationManager.AppSettings
            MerchantName = .Get("MerchantName")
            normalPassword = .Get("normalPassword")
            rebatePassword = .Get("rebatePassword")
            refundPassword = .Get("refundPassword")
            transAccount = .Get("transAccount")
        End With

        Try

            If IsNothing(MerchantName) Then _
                Throw New Exception("The selected environment does not have a Realex account configured.")

            Dim orderID As String
            orderID = Now().ToString("yyyyMMddhhmmss")

            With ConfigurationManager.AppSettings
                tReq = New RealexPayments.RealAuth.TransactionRequest(
                    MerchantName,
                    normalPassword,
                    rebatePassword,
                    refundPassword
                )

                ccReq = New RealexPayments.RealAuth.CreditCard(
                    Request("lstCardType"),
                    txtCardNum.Text,
                    Right("00" & Request("lstMonth"), 2) & Right("00" & Request("lstYear"), 2),
                    txtCardHolder.Text,
                    Request("txtCVN"),
                    RealexPayments.RealAuth.CreditCard.CVN_PRESENT
                )

                Dim part() As String = Split(Request("txtamount"), ".")
                If UBound(part) = 0 Then
                    ReDim Preserve part(1)
                    part(1) = "00"
                Else
                    part(1) = Left(part(1) & "00", 2)
                End If

                tResp = tReq.Authorize(transAccount,
                    orderID,
                    Request("lstCurrency"),
                    CInt(part(0) & part(1)),
                    ccReq
                )
            End With

            If (tResp.ResultCode = 0 Or tResp.ResultCode = 200) Then
                Try
                    Using pay As New paymentLoading
                        Dim p As priRow = pay.AddRow(
                            Request.Params("REF"),
                            Request.Params("CUSTNAME"),
                            Request.Params("TYPE")
                        )

                        pay.TPAYMENT2.AddRow(
                            p,
                            "8",
                            CDbl(txtAmount.Text),
                            Replace(Strings.Space(Len(txtCardNum.Text) - 4) & Right(txtCardNum.Text, 4), " ", ""),
                            "0",
                            tResp.ResultAuthCode,
                            tResp.ResultPASRef
                        )

                        Dim ex As Exception = Nothing
                        pay.Post(ex)
                        If Not TypeOf ex Is apiResponse Then Throw (ex)
                        With TryCast(ex, apiResponse)
                            For Each msg As apiError In .msgs
                                If Not msg.Loaded Then
                                    Throw New Exception(.Message)
                                End If
                            Next

                        End With

                        displayMessage = "Processed Ok."
                        backgroundColour = Drawing.Color.Green

                    End Using

                Catch ex As Exception
                    ' transaction failed
                    displayMessage = "Transaction failed with unhandled exception " & vbCrLf & ex.Message & vbCrLf & "Auth code was: " & tResp.ResultAuthCode
                    backgroundColour = Drawing.Color.Yellow
                    Me.btn.Enabled = True

                End Try

            Else
                Me.btn.Enabled = True

                displayMessage = "Transaction failed. " & vbCrLf & "[" & tResp.ResultCode & "] " & vbCrLf & tResp.ResultMessage
                backgroundColour = Drawing.Color.Red
            End If

        Catch ex As Exception
            ' transaction failed
            displayMessage = ex.Message
            backgroundColour = Drawing.Color.Yellow
            Me.btn.Enabled = True
        End Try

        With Me.txtResult
            .Text = displayMessage
            .BackColor = backgroundColour
        End With

        'Me.btn.Enabled = True
        'Me.btn.Enabled = False

    End Sub

End Class

Public Class paymentLoading : Inherits priForm

    Private _TPAYMENT2 As priForm
    Public Property TPAYMENT2 As priForm
        Get
            Return _TPAYMENT2
        End Get
        Set(value As priForm)
            _TPAYMENT2 = value
        End Set

    End Property

    Sub New()
        MyBase.New("TINVOICES", "ACCNAME", "REFERENCE")
        _TPAYMENT2 = Me.AddForm("TPAYMENT2", "PAYMENTCODE", "QPRICE")

    End Sub

End Class