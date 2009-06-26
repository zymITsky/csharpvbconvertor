#Region "Imported Libraries"
Imports System
Imports Microsoft.VisualStudio.CommandBars
Imports Extensibility
Imports EnvDTE
Imports EnvDTE80
#End Region
Public Class Connect
    Implements IDTExtensibility2

#Region "Private Variables"
    Private isHandlersSet As Boolean
    Private _addInInstance As AddIn
    Public WithEvents VBToCSharpBar As Microsoft.VisualStudio.CommandBars.CommandBarButton
    Public WithEvents CSharpToVBBar As Microsoft.VisualStudio.CommandBars.CommandBarButton
    Public WithEvents CodeWindow As Microsoft.VisualStudio.CommandBars.CommandBar
    Public WithEvents WindowEvents As EnvDTE.WindowEvents
    Public Shared DTE As DTE2
#End Region

#Region "Addin Methods"
    '''<summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
    Public Sub New()
    End Sub
    '''<summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
    '''<param name='application'>Root object of the host application.</param>
    '''<param name='connectMode'>Describes how the Add-in is being loaded.</param>
    '''<param name='addInInst'>Object representing this Add-in.</param>
    '''<remarks></remarks>
    Public Sub OnConnection(ByVal application As Object, ByVal connectMode As ext_ConnectMode, ByVal addInInst As Object, ByRef custom As Array) Implements IDTExtensibility2.OnConnection
        DTE = CType(application, DTE2)
        _addInInstance = CType(addInInst, AddIn)
        WindowEvents = DTE.Events.WindowEvents
        CreateHandlers()
    End Sub
    '''<summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
    '''<param name='disconnectMode'>Describes how the Add-in is being unloaded.</param>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnDisconnection(ByVal disconnectMode As ext_DisconnectMode, ByRef custom As Array) Implements IDTExtensibility2.OnDisconnection
    End Sub
    '''<summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification that the collection of Add-ins has changed.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnAddInsUpdate(ByRef custom As Array) Implements IDTExtensibility2.OnAddInsUpdate
    End Sub
    '''<summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnStartupComplete(ByRef custom As Array) Implements IDTExtensibility2.OnStartupComplete
    End Sub
    '''<summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnBeginShutdown(ByRef custom As Array) Implements IDTExtensibility2.OnBeginShutdown
    End Sub
#End Region

#Region "Private Method Implementation"
    Private Function RetrieveCodeWindow() As Microsoft.VisualStudio.CommandBars.CommandBar
        If CodeWindow Is Nothing Then
            Dim wholeBars As Microsoft.VisualStudio.CommandBars.CommandBars = CType(DTE.CommandBars, Microsoft.VisualStudio.CommandBars.CommandBars)
            CodeWindow = wholeBars.Item("Code Window")
        End If
        Return CodeWindow
    End Function
    Private Sub RemoveHandlers()
        If isHandlersSet Then
            CodeWindow = RetrieveCodeWindow()
            RemoveBarIfExists("VB To CSharp", CodeWindow)
            RemoveBarIfExists("CSharp To VB", CodeWindow)
        End If
    End Sub
    Public Sub CreateHandlers()
        If Not isHandlersSet Then
            CodeWindow = RetrieveCodeWindow()
            VBToCSharpBar = CheckBarForExistence("VB To CSharp", CodeWindow)
            CSharpToVBBar = CheckBarForExistence("CSharp To VB", CodeWindow)
            isHandlersSet = True
        End If
    End Sub
    Private Sub WindowEvents_WindowActivated(ByVal GotFocus As EnvDTE.Window, ByVal LostFocus As EnvDTE.Window) Handles WindowEvents.WindowActivated
        HandleDocument(GotFocus)
    End Sub
    Private Sub WindowEvents_WindowCreated(ByVal GotFocus As EnvDTE.Window) Handles WindowEvents.WindowCreated
        HandleDocument(GotFocus)
    End Sub
    Private Sub HandleDocument(ByVal GotFocus As EnvDTE.Window)
        CreateHandlers()
        If GotFocus.Document IsNot Nothing Then
            If GotFocus.Document.Language = "Basic" Then
                CSharpToVBBar.Visible = False
                VBToCSharpBar.Visible = True
            ElseIf GotFocus.Document.Language = "CSharp" Then
                CSharpToVBBar.Visible = True
                VBToCSharpBar.Visible = False
            Else
                CSharpToVBBar.Visible = False
                VBToCSharpBar.Visible = False
            End If
        End If
    End Sub
    Private Sub VBToCSharpBar_Click(ByVal Ctrl As Microsoft.VisualStudio.CommandBars.CommandBarButton, ByRef CancelDefault As Boolean) Handles VBToCSharpBar.Click
        CodeConvertor.ConvertToCsharpAndVb()
    End Sub
    Private Sub CSharpToVBBar_Click(ByVal Ctrl As Microsoft.VisualStudio.CommandBars.CommandBarButton, ByRef CancelDefault As Boolean) Handles CSharpToVBBar.Click
        CodeConvertor.ConvertToCsharpAndVb()
    End Sub
#End Region

End Class