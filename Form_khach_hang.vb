Option Compare Database
Dim Total As Integer
Dim EditingMode As Boolean

Public Function CheckInput(Name, Email, PhoneNumber, Address) As Integer
    Passed = 4
      
    If (IsNull(Name)) Then
        'MsgBox ("Name error")
        Passed = Passed - 1
        Me.CurNameErr.Visible = True
        Me.CurName.BorderColor = vbRed
    End If
    
    If (IsNull(Email)) Then
        'MsgBox ("Email Error")
        Passed = Passed - 1
        Me.CurEmailErr.Visible = True
        Me.CurEmail.BorderColor = vbRed
    End If
    
    If (IsNull(PhoneNumber)) Then
        'MsgBox ("Phone number error")
        Passed = Passed - 1
        Me.CurPhoneNumberErr.Visible = True
        Me.CurPhoneNumber.BorderColor = vbRed
    End If
    
    If (IsNull(Address)) Then
        'MsgBox ("Address Error")
        Passed = Passed - 1
        Me.CurAddressErr.Visible = True
        Me.CurAddress.BorderColor = vbRed
    End If
    
    CheckInput = Passed
End Function

Public Function ResetErrorMessage()
    Me.CurNameErr.Visible = False
    Me.CurEmailErr.Visible = False
    Me.CurAddressErr.Visible = False
    Me.CurPhoneNumberErr.Visible = False
    
    Me.CurName.BorderColor = vbBlack
    Me.CurEmail.BorderColor = vbBlack
    Me.CurAddress.BorderColor = vbBlack
    Me.CurPhoneNumber.BorderColor = vbBlack
End Function

Public Function HandleUserChange()
    Dim sql As String
    Dim qry As String

    'If (IsNull(Me.UserSearch)) Then
    '    qry = ""
    'Else
        Me.UserSearch.SetFocus
        qry = Me.UserSearch.Text
    'End If
    
    'SELECT [KHACH_HANG].[ID], [KHACH_HANG].[Name], [KHACH_HANG].[Gender], [KHACH_HANG].[Birthdate], [KHACH_HANG].[PhoneNumber], [KHACH_HANG].[Email] FROM KHACH_HANG
    sql = "SELECT ID, Name, Gender, Birthdate, PhoneNumber, Email, Address FROM KHACH_HANG WHERE Name Like '*" & qry & "*' ORDER BY Name"
    
    Me.UserLIst.RowSource = sql
    
    Me.Requery
    Me.UserLIst.Requery
    
    UpdateCounter
End Function


Private Sub DeleteRecord_Click()
    DoCmd.RunSQL ("DELETE FROM KHACH_HANG WHERE ID='" & Me.CurID & "'")
    Me.EditBtt.SetFocus
    HandleUserChange
    GetSelectedUser 0
    ToggleEdit False
End Sub

Private Sub OrderList_DblClick(Cancel As Integer)

    Dim ID, varItem, condition
    
    For Each varItem In Me.OrderList.ItemsSelected
        ID = Me.OrderList.Column(0, varItem)
    Next varItem
    
    If (IsNull(ID) Or Len(ID) = 0) Then Exit Sub
    
    Debug.Print ID
    condition = "[ID_DON] = " & ID
    DoCmd.OpenReport "HOA_DON", acViewPreview, , condition, acDialog
End Sub

Public Function GetSelectedUser(Optional ForceNumber As Integer = 9999)
    Dim varItem As Variant
    Dim ID, Name, Birthdate, PhoneNumber, Email, Address
    
    If (ForceNumber = 9999) Then
        varItem = Me.UserLIst.ItemsSelected(0)
    Else
        varItem = ForceNumber
    End If
    
    Me.EditBtt.Visible = True
    
    ID = Me.UserLIst.Column(0, varItem)
    Name = Me.UserLIst.Column(1, varItem)
    'Gender = Me.UserList.Column(2, varItem)
    Birthdate = Me.UserLIst.Column(3, varItem)
    PhoneNumber = Me.UserLIst.Column(4, varItem)
    Email = Me.UserLIst.Column(5, varItem)
    Address = Me.UserLIst.Column(6, varItem)
    
    Me.CurAddress.ForeColor = vbBlack
    Me.CurAddress.Value = Address
    If (IsNull(Address) Or Len(Address) = 0) Then
        Me.CurAddress.Value = "Unavailable"
        Me.CurAddress.ForeColor = vbOrange
    End If
    
        
    Me.CurPhoneNumber.Value = PhoneNumber
    Me.CurPhoneNumber.ForeColor = vbBlack
    If (IsNull(PhoneNumber) Or Len(PhoneNumber) = 0) Then
        Me.CurPhoneNumber.Value = "Unavailable"
        Me.CurPhoneNumber.ForeColor = vbOrange
    End If
    
    Me.CurEmail.Value = Email
    Me.CurEmail.ForeColor = vbBlack
    If (IsNull(Email) Or Len(Email) = 0) Then
        Me.CurEmail.Value = "Unavailable"
        Me.CurEmail.ForeColor = vbRed
    End If
    
    Me.CurID.Value = ID
    Me.CurName.Value = Name
    Me.CurBirthdate.Value = Birthdate
    
    Dim sql As String
    sql = "SELECT ID_DON, NDH, NGH, ID_KH, Tong_so_luong, Giao_hang, Chi_phi_phu, Tong_tien FROM DON_HANG WHERE ID_KH Like '*" & ID & "*' ORDER BY NDH"
    Me.OrderList.RowSource = sql
    
    Dim PLF, PLS, PLD As String
    
    PLF = Me.PLF.Caption
    PLS = Me.PLS.Caption
    PLD = Me.PLD.Caption

    
    If (Me.OrderList.ListCount = 0) Then
        Me.OrderList.Visible = False
        Me.OrderCount.Caption = ""
    Else
        Me.OrderList.Visible = True
        Me.OrderCount.Caption = PLF + CStr(Me.OrderList.ListCount) + PLS + CStr(Me.OrderList.ListCount) + PLD
    End If
End Function

Public Function UpdateCounter()
    On Error Resume Next
    Dim Count As Integer
    Dim PLF, PLS, PLT As String
    
    PLF = Me.PLF.Caption
    PLS = Me.PLS.Caption
    PLT = Me.PLT.Caption
    
    Total = DCount("ID='*CS*'", "KHACH_HANG")
    
    Count = Me.UserLIst.ListCount
    Me.UserCount.Caption = PLF + CStr(Count) + PLS + CStr(Total) + PLT
End Function

Public Function Restart()
    Dim sql As String
    sql = "SELECT ID, Name, Gender, Birthdate, PhoneNumber, Email, Address FROM KHACH_HANG WHERE (((KHACH_HANG.Name) Like '**')) ORDER BY KHACH_HANG.Name"
    Me.UserLIst.RowSource = sql
    
    
    UpdateCounter
    ToggleEdit False
    ResetErrorMessage
    
    Me.EditBtt.Visible = False
    Me.OrderList.Visible = False
    
    Me.CurID.Value = ""
    Me.CurName.Value = ""
    Me.CurBirthdate.Value = ""
    Me.CurPhoneNumber.Value = ""
    Me.CurEmail.Value = ""
    Me.CurAddress.Value = ""
    
    Me.UserSearch.SetFocus
    Me.UserSearch.Text = ""
End Function

Public Function AbortEditing()
    ResetErrorMessage
    ToggleEdit False
    GetSelectedUser
End Function

Public Function ToggleEdit(ToggleOn As Boolean)
    Dim Style, Enabled, Locked
    
    If (ToggleOn) Then
        Style = 1
        Locked = False
        Enabled = True
        EditingMode = True
        Me.EditBtt.Caption = Me.savePH.Caption
        
        Me.CancelEdit.Visible = True
        Me.DeleteRecord.Visible = True
    Else
        Style = 0
        Locked = True
        Enabled = False
        EditingMode = False
        Me.EditBtt.Caption = Me.editPH.Caption
        
        
        Me.CancelEdit.Visible = False
        Me.DeleteRecord.Visible = False
    End If

    Me.CurName.BackStyle = Style
    Me.CurName.BorderStyle = Style
    Me.CurName.Enabled = Enabled
    Me.CurName.Locked = Locked
    
    Me.CurPhoneNumber.BackStyle = Style
    Me.CurPhoneNumber.BorderStyle = Style
    Me.CurPhoneNumber.Enabled = Enabled
    Me.CurPhoneNumber.Locked = Locked
    
    Me.CurEmail.BackStyle = Style
    Me.CurEmail.BorderStyle = Style
    Me.CurEmail.Enabled = Enabled
    Me.CurEmail.Locked = Locked
    
    Me.CurAddress.BackStyle = Style
    Me.CurAddress.BorderStyle = Style
    Me.CurAddress.Enabled = Enabled
    Me.CurAddress.Locked = Locked
End Function

Private Sub AddCustomerBtt_Click()
    DoCmd.OpenForm "FormThemKhach"
End Sub

Private Sub EditBtt_Click()
    If (EditingMode) Then
        Me.EditBtt.SetFocus
        
        Dim Name, Email, Address, PhoneNumber As String
        Dim Passed As Integer
        
        Name = Me.CurName
        Email = Me.CurEmail
        PhoneNumber = Me.CurPhoneNumber
        Address = Me.CurAddress
        
        Passed = CheckInput(Name, Email, PhoneNumber, Address)
        
        If (Passed < 4) Then
            MsgBox "An error has occured while trying to save new information", vbOKOnly, "Meow Meow Laptop Management Tool"
            Exit Sub
        End If
        
        Dim sql As String
        sql = "SET Name='" & Name & "', Email='" & Email & "', PhoneNumber='" & PhoneNumber & "', Address='" & Address & "' "
        
        DoCmd.RunSQL ("UPDATE KHACH_HANG " & sql & " WHERE ID = '" & Me.CurID & "'")
        DoCmd.OpenQuery "UPDATE_NAME_KH"
        
        ResetErrorMessage
        HandleUserChange
        ToggleEdit False
    Else
        ToggleEdit True
    End If
End Sub

Private Sub CancelEdit_Click()
    Me.EditBtt.SetFocus
    AbortEditing
End Sub

Private Sub Form_Load()
    Restart
End Sub

Private Sub UserList_Click()
    AbortEditing
    'GetSelectedUser
End Sub
    
Private Sub UserLIst_KeyPress(KeyAscii As Integer)
    GetSelectedUser
End Sub

Private Sub UserSearchClear_Click()
    Me.UserSearch.SetFocus
    Me.UserSearch.Text = ""
End Sub

Private Sub UserSearch_Change()
    HandleUserChange
End Sub
        
