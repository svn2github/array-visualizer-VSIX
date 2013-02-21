﻿Imports LinqLib.Array
Imports LinqLib.Sequence

Public Class Form1
  Dim mArr1 As Integer(,)
  Dim mArr2 As Array
  Dim mArrUnused1 As Long(,,)

  Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click

    Dim arr1 As Integer(,,)
    Dim arr2 As Array
    Dim arrUnused1 As Long(,,)
    Dim arr3 As TestType(,)
    Dim arr4 As Array

    Dim sales()() As Double = New Double(11)() {}
    Dim month As Integer
    Dim days As Integer
    For month = 0 To 11
      days = DateTime.DaysInMonth(Date.Today.Year, month + 1)
      sales(month) = New Double(days - 1) {}
    Next month


    mArr1 = Enumerator.Generate(Of Integer)(1, 1, 20).Shuffle().ToArray(4, 5)
    mArr2 = Enumerator.Generate(Of Long)(1, 1, 360).Shuffle().ToArray(3, 3, 4, 5)

    arr1 = Enumerator.Generate(Of Integer)(1, 1, 100).Shuffle().ToArray(5, 5, 4)
    arr2 = Enumerator.Generate(Of Integer)(1, 1, 49).Shuffle().ToArray(7, 7)
    arr3 = Enumerator.Generate(Of TestType)(15, Function(X) GetNewType(X)).Shuffle().ToArray(5, 3)
    arr4 = Enumerator.Generate(Of TestType)(27, Function(X) GetNewType(X)).Shuffle().ToArray(9, 3)

    Dim J2 As Integer()()() = GetJaggedArray2()
    Debugger.Break()
  End Sub

  Private Shared Function GetJaggedArray2() As Integer()()()

    Dim arr()()() As Integer =
              {
               New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}, New Integer() {1, 2, 3}, New Integer() {1, 2}}, _
               New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}, New Integer() {1, 2, 3}}, _
               New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3, 4, 5}, New Integer() {1}}, _
               New Integer()() {New Integer() {1, 2, 3}, _
                                New Integer() {1, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3, 4, 5, 2, 3}, _
                                New Integer() {1}, New Integer() {1, 2, 3}, New Integer() {1, 2}}}
    Return arr

  End Function



  Private Function GetNewType(X As Integer) As TestType

    Dim tt As New TestType()
    tt.a = X
    tt.b = X * 2

    Return tt
  End Function

  Public Class TestType
    Public a As Integer
    Public b As Integer

    Public Overrides Function ToString() As String
      Return String.Format("{0} of {1}", a, b)
    End Function
  End Class
End Class
