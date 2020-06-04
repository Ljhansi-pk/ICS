Imports CooperTire.ICS.Model
Imports CooperTire.ICS.Common
Imports CooperTire.ICS.DomainEntities

''' <summary>
''' Imark Certification view pesenter
''' </summary>
Public Class ImarkCertificationPresenter
    Inherits CertificationPresenterBase

    ' Changed sku to material number as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation. 

#Region "Members"

    Private m_view As IImarkCertificationView

#End Region

#Region "Constructors"

    Public Sub New(ByVal p_view As IImarkCertificationView)

        MyBase.New(p_view)

        m_view = p_view
        SubscribeToEvents()

    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Attach presenter to view�s events.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SubscribeToEvents()

        AddHandler m_view.LoadView, AddressOf OnLoadView
        AddHandler m_view.Renew, AddressOf OnRenew
        AddHandler m_view.Save, AddressOf OnSave
        AddHandler m_view.MoldStampingRefresh, AddressOf OnMoldStampingRefresh

    End Sub

    ''' <summary>
    ''' On load view
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OnLoadView(ByVal sender As Object, ByVal e As EventArgs)

        Try
            If CertificationPresenterBase.FreshStart Then
                ' Reset sensitive Imark properties
                m_view.ToBeRenewedCertificate = Nothing
            End If
        Catch exp As Exception
            EventLogger.Enter(exp)
            m_view.ErrorText = "Renewal setup error."
        End Try

    End Sub

    ''' <summary>
    ''' On save view
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overloads Sub OnSave()

        Try
            ' Result of save from the base
            If m_enumSaveResult = NameAid.SaveResult.Sucess Then
                ' Drop after save success
                m_view.ToBeRenewedCertificate = Nothing
            End If
        Catch exp As Exception
            EventLogger.Enter(exp)
            m_view.ErrorText = "Renewal setup error."
        End Try

    End Sub

    ''' <summary>
    ''' On renew Imark certificate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnRenew()

        m_view.InfoText = String.Empty
        m_view.ErrorText = String.Empty
        Dim enumSaveResult As NameAid.SaveResult

        Try
            'Only update those Material numbers that have already been certified.
            enumSaveResult = RenewCertificate()
            m_view.InfoText = "Imark Certificate Renewed"

        Catch exp As Exception
            EventLogger.Enter(exp)
            m_view.ErrorText = "Renewal failed."
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RenewCertificate() As NameAid.SaveResult

        ' Only update those Material numbers that have already been certified, increment extension
        Dim enumSaveResult As NameAid.SaveResult
        Dim objImarkCertModel As New ImarkCertificateModel
        Dim intNewImarkCertificateId As Integer

        enumSaveResult = objImarkCertModel.RenewCertificate(m_view.CertificateNumberID, intNewImarkCertificateId)

        Return enumSaveResult

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_certRenewal"></param>
    ''' <remarks></remarks>
    Protected Overloads Sub LoadCertificateData(ByVal p_certRenewal As Certificate)

        MapCertificateToView(p_certRenewal)

        m_view.OriginalCertificate = Nothing
        m_view.ToBeRenewedCertificate = p_certRenewal.ToBeRenewedCertificate_I

        m_view.InfoText = String.Empty
        m_view.ErrorText = String.Empty
        m_view.SetupControlContextState(ICertificationView.Context.NewCertificate)

    End Sub

    ''' <summary>
    ''' Map view data To Certificate entity
    ''' </summary>
    ''' <returns>null in case of failure</returns>
    ''' <remarks></remarks>
    Protected Overrides Function MapViewToCertificate() As Certificate

        Dim objCertificate As Certificate = Nothing

        Try
            objCertificate = New Certificate()

            objCertificate.MaterialNumber = m_view.MaterialNumber
            objCertificate.SKUID = m_view.SKUID
            objCertificate.RemoveMatlNum = m_view.RemoveMatlNum
            objCertificate.CertificationTypeName = "Imark"
            objCertificate.Extension_EN = m_view.ExtensionNo

            
            objCertificate.CertificateNumber = m_view.CertificationNumber '4/12/16 jeseitz ImarkCertificateModel.ImarkCertNumber
            objCertificate.CertificateNumberID = m_view.CertificateNumberID

            objCertificate.Family_I = m_view.ImarkFamily
            objCertificate.MoldStamping = m_view.ImarkStamping
            objCertificate.EmarkReference_I = m_view.EmarkReference

            If Not String.IsNullOrEmpty(m_view.DateAssigned) Then
                objCertificate.DateAssigned_EGI = CType(m_view.DateAssigned, DateTime)
            End If

            If Not String.IsNullOrEmpty(m_view.CertDateApproved) Then
                objCertificate.CertDateApproved_CEGI = CType(m_view.CertDateApproved, DateTime)
            End If

            If Not String.IsNullOrEmpty(m_view.DateApproved) Then
                objCertificate.DateApproved_CEGI = CType(m_view.DateApproved, DateTime)
            End If

            If Not String.IsNullOrEmpty(m_view.CertDateSubmitted) Then
                objCertificate.CertDateSubmitted = CType(m_view.CertDateSubmitted, DateTime)
            End If

            If Not String.IsNullOrEmpty(m_view.DateSubmitted) Then
                objCertificate.DateSubmitted = CType(m_view.DateSubmitted, DateTime)
            End If

            If Not String.IsNullOrEmpty(m_view.DateExpiry) Then
                objCertificate.ExpiryDate_I = CType(m_view.DateExpiry, DateTime)
            End If

            objCertificate.ActiveStatus = m_view.ActiveStatus
            objCertificate.RenewalRequired_CGIN = m_view.RenewalRequired

            objCertificate.OriginalCertificate = m_view.OriginalCertificate
            objCertificate.ToBeRenewedCertificate_I = m_view.ToBeRenewedCertificate

            'JBH_2.00 Project 5325 - Mold Change Required Checkbox
            objCertificate.MoldChgRequired = m_view.MoldChgRequired

            'JBH_2.00 Project 5325 - Operation Approval Date
            If Not String.IsNullOrEmpty(m_view.OperDateApproved) Then
                objCertificate.OperDateApproved = CType(m_view.OperDateApproved, DateTime)
            Else
                objCertificate.OperDateApproved = Date.MinValue
            End If

            'jeseitz 10/29/2016
            objCertificate.AddInfo = m_view.AddInfo


        Catch exc As Exception
            objCertificate = Nothing
            EventLogger.Enter(exc)
        End Try

        Return objCertificate

    End Function

    ''' <summary>
    ''' Map Certificate to view properties
    ''' </summary>
    ''' <param name="p_objCertificate"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub MapCertificateToView(ByVal p_objCertificate As Certificate)

        ' IMARK CertificateNumber appears to the user the same, regardless of renewals
        m_view.CertificationNumber = p_objCertificate.CertificateNumber '4/12/16 jeseitz ImarkCertificateModel.ImarkCertNumber
        m_view.ExtensionNo = p_objCertificate.Extension_EN

        'Get the data for the manufacturing locations drop-down list
        Dim certModel As CertificateModel = New CertificateModel()
        Dim dstLocs As DataSet = certModel.GetManufacturingLocationsList

        m_view.DataBindView()

        If Not String.IsNullOrEmpty(p_objCertificate.CertificateNumber) Then
            'jeseitz 4/13/2016 m_view.CertificationNumber = p_objCertificate.CertificateNumber.Substring(0, 4)
            m_view.CertificationNumber = p_objCertificate.CertificateNumber
        End If
        m_view.CertificateNumberID = p_objCertificate.CertificateNumberID

        m_view.DateAssigned = String.Empty
        If Not p_objCertificate.DateAssigned_EGI.Equals(DateTime.MinValue) Then
            m_view.DateAssigned = p_objCertificate.DateAssigned_EGI.ToShortDateString()
        End If

        m_view.CertDateSubmitted = String.Empty
        If Not p_objCertificate.CertDateSubmitted.Equals(DateTime.MinValue) Then
            m_view.CertDateSubmitted = p_objCertificate.CertDateSubmitted.ToShortDateString()
        End If

        m_view.DateSubmitted = String.Empty
        If Not p_objCertificate.DateSubmitted.Equals(DateTime.MinValue) Then
            m_view.DateSubmitted = p_objCertificate.DateSubmitted.ToShortDateString()
        End If

        m_view.CertDateApproved = String.Empty
        If Not p_objCertificate.CertDateApproved_CEGI.Equals(DateTime.MinValue) Then
            m_view.CertDateApproved = p_objCertificate.CertDateApproved_CEGI.ToShortDateString()
        End If

        m_view.DateApproved = String.Empty
        If Not p_objCertificate.DateApproved_CEGI.Equals(DateTime.MinValue) Then
            m_view.DateApproved = p_objCertificate.DateApproved_CEGI.ToShortDateString()
        End If

        'Remove Material number checkbox
        m_view.RemoveMatlNum = p_objCertificate.RemoveMatlNum

        m_view.DateExpiry = String.Empty
        If Not p_objCertificate.ExpiryDate_I.Equals(DateTime.MinValue) Then
            m_view.DateExpiry = p_objCertificate.ExpiryDate_I.ToShortDateString()
        End If

        'Set selection for Manufacturing Location drop-down
        m_view.ManufacturingLocationId = 0
     

        m_view.ActiveStatus = p_objCertificate.ActiveStatus
        m_view.RenewalRequired = p_objCertificate.RenewalRequired_CGIN
        m_view.EmarkReference = p_objCertificate.EmarkReference_I
        m_view.ImarkFamily = p_objCertificate.Family_I
        m_view.ImarkStamping = p_objCertificate.MoldStamping
        'ProductData
        m_view.ProductData = String.Concat(p_objCertificate.lblSizeStamp, Chr(9), p_objCertificate.lblSingLoadIndex, "/", p_objCertificate.lblDualLoadIndex, p_objCertificate.lblSpeedRating, Chr(9), p_objCertificate.lblBrandDesc, Chr(9), IIf(p_objCertificate.lblTubelessYN.ToUpper.Equals("Y"), "Tubeless", "Tube"), Chr(9), p_objCertificate.TPN)

        'Added as per project 2706 technical specification
        'Discontinued Date
        If p_objCertificate.DiscontinuedDate.Equals(DateTime.MinValue) Then
            m_view.DiscDate = String.Empty
        Else
            m_view.DiscDate = p_objCertificate.DiscontinuedDate.ToShortDateString()
        End If

        'JBH_2.00 Project 5325 - Mold Change Required Checkbox
        m_view.MoldChgRequired = p_objCertificate.MoldChgRequired

        'JBH_2.00 Project 5325 - Operation Approval Date
        If p_objCertificate.OperDateApproved.Equals(DateTime.MinValue) Then
            m_view.OperDateApproved = String.Empty
        Else
            m_view.OperDateApproved = p_objCertificate.OperDateApproved.ToShortDateString()
        End If

        'jeseitz 10/29/2016 Req 203625
        m_view.AddInfo = p_objCertificate.AddInfo


    End Sub

    ''' <summary>
    ''' Display changes to client
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub DisplayChanges()

        m_view.DisplayChangesToClient()

    End Sub

    ''' <summary>
    ''' Get the Family Desc and Check for Family Id Exist
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub OnMoldStampingRefresh()
        Dim strIMarkFamily As String = m_view.ImarkFamily
        Dim strIMarkFamilyDesc As String = m_view.ImarkFamily
        Dim intCertificateid As Integer = m_view.CertificateNumberID 'jeseitz 4/11/2016
        m_view.InfoText = String.Empty
        m_view.ErrorText = String.Empty

        If Not String.IsNullOrEmpty(strIMarkFamily) Then
            If IsFamilyExist(intCertificateid, strIMarkFamily, strIMarkFamilyDesc) Then
                m_view.ImarkStamping = strIMarkFamilyDesc
            Else
                m_view.ErrorText = "IMark Family does not exist."
                m_view.ImarkStamping = String.Empty
            End If
        End If
    End Sub
#End Region

End Class
