Imports System.IO

Public Class Form1

    'Class level variabele
    'Eenmalige initialisatie
    'De scope van de variabele is nu vergroot naar de gehele class
    Dim allPeople As New List(Of Person)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MessageBox.Show($"Hallo {txtFirstname.Text} {txtLastname.Text}")
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtFirstname.Text = ""
        txtLastname.Text = ""
        dtpDateOfBirth.Value = DateTime.Now
        cbxCountry.Text = ""
        rbUnknown.Checked = True
    End Sub

    Private Sub btnShowDateTime_Click(sender As Object, e As EventArgs) Handles btnShowDateTime.Click
        MessageBox.Show(dtpDateOfBirth.Value.ToString("dd-MM-yyyy hh:mm:ss"))
    End Sub

    Private Sub btnAddPerson_Click(sender As Object, e As EventArgs) Handles btnAddPerson.Click

        Try

            Dim p As New Person
            p.Firstname = txtFirstname.Text
            p.Lastname = txtLastname.Text
            p.DateOfBirth = dtpDateOfBirth.Value
            p.Country = cbxCountry.Text

            If rbMale.Checked Then
                p.Gender = GenderType.Male
            End If

            If rbFemale.Checked Then
                p.Gender = GenderType.Female
            End If

            If rbUnknown.Checked Then
                p.Gender = GenderType.Unknown
            End If

            'Dit werkt bijna goed! Na lunch nadenken over waarom er maar 1 persoon
            'kan worden toegevoegd...

            allPeople.Add(p)

            'Let op dat de datagridview niet zomaar refresht!
            'Ontkoppel de verzameling eerst van de dgv
            'En koppel de lijst dan weer aan de DataSource property
            DataGridView1.DataSource = Nothing
            DataGridView1.DataSource = allPeople

        Catch ex As Exception
            'Hier wordt foutafhandeling gedaan
            MessageBox.Show(ex.Message)

        End Try

    End Sub

    Private Sub SaveCSVToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveCSVToolStripMenuItem.Click

        Dim contents As String = ""

        For Each p In allPeople
            contents += String.Join(";", p.Firstname, p.Lastname, p.DateOfBirth, p.Gender, p.Country) + Environment.NewLine
        Next

        'Maak voor nu die tmp folder even handmatig aan....
        'Straks gaan we met een dialoog de gebruiker de juiste file zelf laten kiezen

        Dim sfd As New SaveFileDialog
        sfd.InitialDirectory = "c:\tmp"
        sfd.Filter = "Csv Files|*.csv|Text files|*.txt"

        sfd.ShowDialog()

        File.WriteAllText(sfd.FileName, contents)

    End Sub

    Private Sub LoadCSVToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadCSVToolStripMenuItem.Click
        Dim ofd As New OpenFileDialog
        ofd.InitialDirectory = "c:\tmp"
        ofd.Filter = "Csv Files|*.csv|Text files|*.txt"

        If ofd.ShowDialog() = DialogResult.Cancel Then
            Return
        End If

        Dim contents As String() = File.ReadAllLines(ofd.FileName)

        For Each line As String In contents
            Dim fields As String() = line.Split(";")

            Dim p As New Person
            p.Firstname = fields(0)
            p.Lastname = fields(1)
            p.DateOfBirth = fields(2)

            'Dim waarden = [Enum].GetValues(GetType(GenderType))

            Select Case fields(3)
                Case "Male"
                    p.Gender = GenderType.Male
                Case "Female"
                    p.Gender = GenderType.Female
                Case "Unknown"
                    p.Gender = GenderType.Unknown
            End Select

            p.Country = fields(4)

            allPeople.Add(p)
        Next

        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = allPeople
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If MessageBox.Show("Vergeet niet uw data op te slaan! Wilt U echt afsluiten?", "Persons", MessageBoxButtons.YesNo) = DialogResult.No Then
            e.Cancel = True
        End If

    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        'In deze event is het formulier al gesloten. Dus te laat om nog in het formulier te blijven
        'e.Cancel bestaat hier ook niet...

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Zoek alle mannen met LINQ

        Dim query = From p In allPeople
                    Where p.Gender = GenderType.Male
                    Select p
                    Order By p.Age

        Dim results = query.ToList()

        DataGridView1.DataSource = results

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim query = From p In allPeople
                    Where p.Firstname.ToLower.Contains(txtSearch.Text.ToLower) _
                    Or p.Lastname.ToLower.Contains(txtSearch.Text.ToLower)
                    Select p

        DataGridView1.DataSource = query.ToList
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim query = From p In allPeople
                    Where p.Firstname.ToLower.Contains(txtSearch.Text.ToLower) _
                    Or p.Lastname.ToLower.Contains(txtSearch.Text.ToLower)
                    Select p

        DataGridView1.DataSource = query.ToList
    End Sub
End Class
