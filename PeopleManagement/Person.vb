Public Class Person
    Public Property Firstname As String
    Public Property Lastname As String

    'De geboortedatum moet in het verleden liggen!
    'Maak dus een private variabele en een public property
    'De public property is 'van buitenaf' te benaderen, dus vanuit bv het formulier
    'De private variabele fungeert als daadwerkelijke interne opslag van de variabele
    'Encapsulation: het beschermen van de waarde van een eigenschap
    Private _dateOfBirth As DateTime

    'Vauit het formulier is alleen de public property zichtbaar
    Public Property DateOfBirth As DateTime
        Get
            Return _dateOfBirth
        End Get

        Set(value As DateTime)
            If value < DateTime.Now Then
                _dateOfBirth = value
            Else
                Throw New Exception("Dit is geen geldige datum!")
            End If
        End Set

    End Property



    Public Property Gender As GenderType
    Public Property Country As String

    'Een readonly property is een berekend veld
    'Er is dan alleen een Get en geen Set
    Public ReadOnly Property Age As Integer
        Get
            Dim leeftijd As Integer = DateTime.Now.Year - DateOfBirth.Year

            If DateOfBirth.AddYears(leeftijd) > DateTime.Today Then
                leeftijd -= 1
            End If

            Return leeftijd
        End Get
    End Property

End Class


Public Enum GenderType
    Unknown
    Male
    Female
End Enum


