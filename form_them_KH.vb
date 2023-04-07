
Option Compare Database

'----- Function: Check user input -----'
Public Function CheckInput(Name, Birthdate, Email, PhoneNumber, Address) As Integer
    Passed = 5
      
    If (IsNull(Name)) Then
        'MsgBox ("Name error")
        Passed = Passed - 1
        Me.NameWarning.Visible = True
    End If
    
    If (IsNull(Birthdate)) Then
        'MsgBox ("Gender Error")
        Passed = Passed - 1
        Me.BirthdateWarning.Visible = True
    End If
    
    If (IsNull(Email)) Then
        'MsgBox ("Email Error")
        Passed = Passed - 1
        Me.EmailWarning.Visible = True
    End If
    
    If (IsNull(PhoneNumber)) Then
        'MsgBox ("Phone number error")
        Passed = Passed - 1
        Me.SDTWarning.Visible = True
    End If
    
    If (IsNull(Address)) Then
        'MsgBox ("Address Error")
        Passed = Passed - 1
        Me.AddressWarning.Visible = True
    End If
    
    CheckInput = Passed
End Function

'----- Function: Confirm close action -----'
Public Function ConfirmClosing()
    On Error GoTo ErrorHandler
    
    Dim Passed, Name, Email, PhoneNumber, Address, Birthdate
    
    Name = Me.NewName.Value
    Birthdate = Me.NewBirthdate.Value
    Email = Me.NewEmail.Value
    PhoneNumber = Me.NewSDT.Value
    Address = Me.NewAddress.Value
    
    Passed = CheckInput(Name, Birthdate, Email, PhoneNumber, Address)
        
        
    If (Passed) Then
        Dim Prompt, Title, Style, Result
        
        Prompt = "Canceling adding a customer, proceed?"
        Title = "Meow Meow Laptop Management Tool"
        Style = vbExclamation + vbYesNo
        Result = MsgBox(Prompt, Style, Title)
        
        If (Result = 7) Then
            ConfirmClosing = False
            Exit Function
        End If
        
    End If
    
    ConfirmClosing = True
    
ErrorHandler:
 ConfirmClosing = True
 
End Function

'----- Close window -----'
Private Sub CancelBtt_Click()
    'Dim Confirm As Boolean
    'Confirm = ConfirmClosing()
    
    'If (Confirm) Then
        DoCmd.Close
    'End If
End Sub

'----- Form onUnload event -----'
Private Sub Form_Unload(Cancel As Integer)
    Dim Confirm As Boolean
    Confirm = ConfirmClosing()
    
    If (Not Confirm) Then
        Cancel = True
    End If
End Sub

'----- Confirm onClick event -----'
Private Sub ConfirmBtt_Click()
    Dim Passed, ID, Name, Email, PhoneNumber, Address, Gender, GenderVal
    
    ID = Me.NewID.Value
    Name = Me.NewName.Value
    Birthdate = Me.NewBirthdate.Value
    Email = Me.NewEmail.Value
    PhoneNumber = Me.NewSDT.Value
    Address = Me.NewAddress.Value
    GenderVal = Me.NewGender.Value
    
    Gender = "Nu"
    If (GenderVal = 1) Then Gender = "Nam"
        
    Passed = CheckInput(Name, Birthdate, Email, PhoneNumber, Address)

    If (Passed < 5) Then
        MsgBox "An error has occured while trying to add new customer data", vbOKOnly + vbExclamation, "Meow Meow Laptop Management Tool"
        Exit Sub
    End If
    
    Dim Msg, Style, Title, Result, Col, Val
        
    Msg = "You are about to add a customer, proceed?"
    Style = vbYesNo + vbExclamation
    Title = "Meow Meow Laptop Management Tool"
    
    Result = 6 'MsgBox(Msg, Style, Title)
    
    If (Result = 6) Then
        'Col = "INSERT INTO Khach_Hang (Name, Gender, PhoneNumber, Email, Address)"
        'Val = "VALUES ('" + Name + "', '" + Gender + "','" + PhoneNumber + "', '" + Email + "', '" + Address + "') "
        
        Col = "INSERT INTO Khach_Hang (ID, Name, Gender, Birthdate, PhoneNumber, Email, Address) "
        Val = "VALUES ('" & ID & "', '" & Name & "', '" & Gender & "','" & Birthdate & "','" & PhoneNumber & "', '" & Email & "', '" & Address & "')"
        'Debug.Print Col + Val
        
        DoCmd.RunSQL (Col + Val)
        
        Dim Msg2, Style2, WillEditNext
        
        'Msg2 = "New customer added to the database, continue adding?"
        'Style2 = vbYesNo + vbInformation
        
        'WillEditNext = MsgBox(Msg2, Style2, Title)
        
        Me.NewName.Value = Null
        Me.NewBirthdate.Value = Null
        Me.NewEmail.Value = Null
        Me.NewSDT.Value = Null
        Me.NewAddress.Value = Null
            
        'If (WillEditNext = 7) Then
            DoCmd.Close
        'End If
        
    End If
    
End Sub

'----- Active warning features -----'
Private Sub NewName_LostFocus()
    Me.NameWarning.Visible = False
    If (IsNull(Me.NewName.Value)) Then
        Me.NameWarning.Visible = True
    End If
End Sub

Private Sub NewBirthdate_LostFocus()
    Me.BirthdateWarning.Visible = False
    If (IsNull(Me.NewBirthdate.Value)) Then
        Me.BirthdateWarning.Visible = True
    End If
End Sub

Private Sub NewEmail_LostFocus()
    Me.EmailWarning.Visible = False
    If (IsNull(Me.NewEmail.Value)) Then
        Me.EmailWarning.Visible = True
    End If
End Sub

Private Sub NewSDT_LostFocus()
    Me.SDTWarning.Visible = False
    If (IsNull(Me.NewSDT.Value)) Then
        Me.SDTWarning.Visible = True
    End If
End Sub

Private Sub NewAddress_LostFocus()
    Me.AddressWarning.Visible = False
    If (IsNull(Me.NewAddress.Value)) Then
        Me.AddressWarning.Visible = True
    End If
End Sub

'----- Disable warning feature -----'

Private Sub Form_Load()
    Me.NameWarning.Visible = False
    Me.BirthdateWarning.Visible = False
    Me.EmailWarning.Visible = False
    Me.SDTWarning.Visible = False
    Me.AddressWarning.Visible = False
End Sub

Private Sub NewName_Change()
    Me.NameWarning.Visible = False
End Sub
Private Sub NewBirthdate_Change()
    Me.BirthdateWarning.Visible = False
End Sub

Private Sub NewEmail_Change()
    Me.EmailWarning.Visible = False
End Sub

Private Sub NewSDT_Change()
    Me.SDTWarning.Visible = False
End Sub
Private Sub NewAddress_Change()
    Me.AddressWarning.Visible = False
End Sub


