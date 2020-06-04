Imports System.Data

Imports CooperTire.ICS.DataAccess
Imports TRACSSharedDatasets
Imports CooperTire.ICS.DomainEntities
Imports CooperTire.ICS.Common
Imports CooperTire.ICS.Datasets

''' <summary>
''' Handles the passing of call to appropriate data source - ICS or SKUTRACS
''' </summary>
''' <remarks></remarks>
Public Class DataDepository
    Implements IDepository

    ' Modified as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    ' Used Material Number instead of SKU, PSN instead of NPRId, TPN instead of PPN and Brand, Brand Line instead of Brand Code.
    ' Added Operation as paramter for HDR save methods.

#Region " IDisposable Support "

    Private disposedValue As Boolean = False ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free managed resources when explicitly called
            End If

        ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

    ''' <summary>
    ''' Pass the call to respective data source
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function GetRequestedTests(ByVal p_iCertificationTypeId As Integer, ByVal p_iTireTypeId As Integer, ByVal p_dstClientRequest As DataSet) As DataSet Implements IDepository.GetRequestedTests

        Dim dstTRACStoICSDataset As New TRACStoICSDataset
        Dim objTRACSSupportBus As TRACSSupportBLL.Business = New TRACSSupportBLL.Business
        Dim blnSuccess As Boolean = False

        'Added Properties as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation.
        Dim strUseSap As String = CooperTire.ICS.Common.AppSettingsAid.GetUseSAP()
        Dim strUseTracs As String = CooperTire.ICS.Common.AppSettingsAid.GetUseTracs()

        dstTRACStoICSDataset = objTRACSSupportBus.GetClientTests(p_iCertificationTypeId, p_iTireTypeId, strUseSap, strUseTracs, p_dstClientRequest, blnSuccess)
        Return dstTRACStoICSDataset

    End Function

    Public Function GetAuditLog() As DataSet Implements IDepository.GetAuditLog

        Dim dstResults As New DataSet
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetAuditLog()
        End Using
        Return dstResults

    End Function

    Public Function GetApprovalReasons(ByVal p_intCertificationTypeId As Integer) As DataSet Implements IDepository.GetApprovalReasons

        Dim dstApprovalReasons As New DataSet
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstApprovalReasons = ctdCertDalc.GetApprovalReasons(p_intCertificationTypeId)
        End Using
        Return dstApprovalReasons

    End Function

    Public Function GetApprovedSubstitution(ByVal p_intCertificationTypeId As Integer, ByVal p_strField As String, ByVal p_sngValue As Single, ByVal p_intSKUID As Integer) As Single Implements IDepository.GetApprovedSubstitution

        Dim intNewValue As Single

        If p_sngValue > 0 Then
            Using ctdCertDalc As CertificationDalc = New CertificationDalc
                intNewValue = ctdCertDalc.GetApprovedSubstitution(p_intCertificationTypeId, p_strField, p_sngValue, p_intSKUID)
            End Using
        Else
            intNewValue = p_sngValue
        End If

        Return intNewValue
    End Function

    Public Function GetAuditLogAfterDate(ByVal p_dtmChangeDateTime As Date) As System.Data.DataSet Implements IDepository.GetAuditLogAfterDate

        Dim dstResults As New DataSet
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetAuditLogAfterDate(p_dtmChangeDateTime)
        End Using
        Return dstResults

    End Function

    Public Function GetQueryControlGridSource() As DataTable Implements IDepository.GetQueryControlGridSource
        Dim dtbGridSource As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbGridSource = ctdCertDalc.GetQueryControlGridSource()
        End Using

        Return dtbGridSource
    End Function

    Public Function UpdateAuditLogEntry(ByVal p_intChangeLogID As Integer, ByVal p_dtmChangeDateTime As DateTime, ByVal p_strApprovalStatus As String, ByVal p_strApprover As String) As Boolean Implements IDepository.UpdateAuditLogEntry

        Dim blnSaved As Boolean = False
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnSaved = ctdCertDalc.UpdateAuditLogEntry(p_intChangeLogID, p_dtmChangeDateTime, p_strApprovalStatus, p_strApprover)
        End Using
        Return blnSaved

    End Function

    ''' <summary>
    ''' Pass the call to respective data source
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' returns product data from ICS table
    Public Function GetProductData_SKUTRACS(ByVal strMatlNum As String) As TRACSSharedDatasets.SKUtoICSDataset Implements IDepository.GetProductData_SKUTRACS

        Dim dstSKUtoICSDataset As SKUtoICSDataset = Nothing
        Dim objTRACSSupportBus As TRACSSupportBLL.Business = New TRACSSupportBLL.Business

        Dim blnSuccess As Boolean = False
        dstSKUtoICSDataset = objTRACSSupportBus.GetProductData(strMatlNum, blnSuccess)

        Return dstSKUtoICSDataset

    End Function

    ''' <summary>
    ''' Pass the call to respective data source
    ''' </summary>
    ''' <param name="intCertType"></param>
    ''' <param name="intTireType"></param>
    ''' <param name="strMatlNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTRACSData(ByVal intCertType As Integer, ByVal intTireType As Integer, ByVal strMatlNum As String, ByVal intManufacturingLocationId As Integer) As TRACSSharedDatasets.TRACStoICSDataset Implements IDepository.GetTRACSData

        Dim dstTRACStoICSDataset As TRACStoICSDataset = Nothing
        Dim objTRACSSupportBus As TRACSSupportBLL.Business = New TRACSSupportBLL.Business
        Dim blnSuccess As Boolean = False

        'Added Properties as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation.
        Dim strUseSap As String = CooperTire.ICS.Common.AppSettingsAid.GetUseSAP()
        Dim strUseTracs As String = CooperTire.ICS.Common.AppSettingsAid.GetUseTracs()

        dstTRACStoICSDataset = objTRACSSupportBus.GetTRACSData(intCertType, intTireType, strMatlNum, intManufacturingLocationId, strUseSap, strUseTracs, blnSuccess)

        Return dstTRACStoICSDataset

    End Function

    ''' <summary>
    ''' Pass the call to respective data source
    ''' </summary>
    ''' <param name="p_intCertType"></param>
    ''' <param name="p_strInMatlNum"></param>
    ''' <param name="p_strSimilarMatlNum"></param>
    ''' <param name="p_strMessage"></param>
    ''' <remarks></remarks>
    Public Function CheckSimilarTire(ByVal p_intCertType As Integer, ByVal p_strInMatlNum As String, ByRef p_strSimilarMatlNum As String, ByRef p_intImarkFamily As Integer, ByRef p_strECEReference As String, ByRef p_strMessage As String) As Integer Implements IDepository.CheckSimilarTire

        Dim li_result As Integer

        Using ctdCertificationDalc As CertificationDalc = New CertificationDalc
            li_result = ctdCertificationDalc.CheckSimilarTire(p_intCertType, p_strInMatlNum, p_strSimilarMatlNum, p_intImarkFamily, p_strECEReference, p_strMessage)
        End Using

        Return li_result

    End Function

    Public Function GetDefaultValues(ByVal p_strCertificateType As String, ByVal p_intCertificateNumberID As Integer, ByRef p_strCertificateNumber As String) As System.Data.DataSet Implements IDepository.GetDefaultValues

        Dim dstResults As New DataSet
        Using ctdDefaultValuesDalc As DefaultValuesDalc = New DefaultValuesDalc
            dstResults = ctdDefaultValuesDalc.GetDefaultValues(p_strCertificateType, p_intCertificateNumberID, p_strCertificateNumber)
        End Using
        Return dstResults

    End Function

    Public Function CertificationTypeDefaultValueSave(ByVal p_certDeftValues As List(Of CertificationDefaultField)) As Boolean Implements IDepository.CertificateDefaultvalueSave

        Dim blnSaved As Boolean = False
        Using ctdDefaultValuesDalc As DefaultValuesDalc = New DefaultValuesDalc
            blnSaved = ctdDefaultValuesDalc.CertificationTypeDefaultValueSave(p_certDeftValues)
        End Using
        Return blnSaved

    End Function

    Public Function CertificateDefaultValueSave(ByVal p_certDeftValues As List(Of CertificationDefaultField), ByVal p_certificateNo As String) As Boolean Implements IDepository.CertificateValueSave

        Dim blnSaved As Boolean = False
        Using ctdDefaultValuesDalc As DefaultValuesDalc = New DefaultValuesDalc
            blnSaved = ctdDefaultValuesDalc.CertificateDefaultValueSave(p_certDeftValues, p_certificateNo)
        End Using
        Return blnSaved

    End Function

    ''' <summary>
    ''' Gets the data for emark passenger report.
    ''' </summary>
    ''' <param name="p_strCertificateNumber">The certificate number.</param>
    ''' <param name="p_intCertificationTypeId">The certification type id.</param>
    ''' <param name="p_strExtension">The extension.</param>
    ''' <returns></returns>
    Public Function GetDataForEmarkPassengerReport(ByVal p_strCertificateNumber As String, _
                                            ByVal p_intCertificationTypeId As Integer, _
                                            ByVal p_strExtension As String, _
                                            ByVal p_intTireTypeID As Integer) As DataSet Implements IDepository.GetDataForEmarkPassengerReport

        Dim dstResults As New DataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForEmarkPassengerReport(p_strCertificateNumber, p_intCertificationTypeId, p_strExtension, p_intTireTypeID)
        End Using
        Return dstResults

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_strCertificateNumber"></param>
    ''' <param name="p_intCertificationTypeId"></param>
    ''' <param name="p_strExtension"></param>
    ''' <param name="p_intTireTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForEmarkE117Report(ByVal p_strCertificateNumber As String, _
                                        ByVal p_intCertificationTypeId As Integer, _
                                        ByVal p_strExtension As String, _
                                        ByVal p_intTireTypeID As Integer) As DataSet Implements IDepository.GetDataForEmarkE117Report

        Dim dstResults As New DataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForEmarkE117Report(p_strCertificateNumber, p_intCertificationTypeId, p_strExtension, p_intTireTypeID)
        End Using
        Return dstResults

    End Function

    Public Function GetDataForEmarkReportWithTR(ByVal p_strCertificateNumber As String, _
                                                ByVal p_intTireTypeID As Integer) As DataSet Implements IDepository.GetDataForEmarkReportWithTR

        Dim dstResults As New DataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForEmarkWithTR(p_strCertificateNumber, p_intTireTypeID)
        End Using
        Return dstResults

    End Function

    ''' <summary>
    ''' Get CCC Sequential Report data
    ''' </summary>
    ''' <param name="p_strCertificateNumber"></param>
    ''' <param name="p_intCertificationTypeId"></param>
    ''' <param name="p_strExtension"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForCCCReport(ByVal p_strCertificateNumber As String, _
                                        ByVal p_intCertificationTypeId As Integer, _
                                        ByVal p_strExtension As String) As CCCSequentialDataSet Implements IDepository.GetDataForCCCReport

        Dim dstResults As New CCCSequentialDataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForCCCReport(p_strCertificateNumber, p_intCertificationTypeId, p_strExtension)
        End Using
        Return dstResults

    End Function

    ''' <summary>
    ''' Get CCC Product Description Report data
    ''' </summary>
    ''' <param name="p_strCertificateNumber"></param>
    ''' <param name="p_intCertificationTypeId"></param>
    ''' <param name="p_strExtension"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForCCCProductDescriptionReport(ByVal p_strCertificateNumber As String, _
                                        ByVal p_intCertificationTypeId As Integer, _
                                        ByVal p_strExtension As String) As CCCProductDescriptionDataSet Implements IDepository.GetDataForCCCProductDescriptionReport

        Dim dstResults As New CCCProductDescriptionDataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForCCCProductDescriptionReport(p_strCertificateNumber, p_intCertificationTypeId, p_strExtension)
        End Using
        Return dstResults

    End Function

    ''' <summary>
    ''' Gets the data for GSO passenger report.
    ''' </summary>
    ''' <param name="p_strCertificateNumber"></param>
    ''' <param name="p_intCertificationTypeId"></param>
    ''' <param name="p_strExtension"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForGSOPassengerReport(ByVal p_strCertificateNumber As String, _
                                            ByVal p_intCertificationTypeId As Integer, _
                                            ByVal p_strExtension As String, _
                                            ByVal p_intTireTypeId As Integer) As GSOCertificateDataSet Implements IDepository.GetDataForGSOPassengerReport

        Dim dstResults As New GSOCertificateDataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForGSOPassengerReport(p_strCertificateNumber, p_intCertificationTypeId, p_strExtension, p_intTireTypeId)
        End Using
        Return dstResults

    End Function

    Public Function GetDataForGSOConformityReport(ByVal p_strBatchNumber As String, _
                                            ByVal p_intCertificationTypeId As Integer, _
                                            ByVal p_intTireTypeId As Integer) As DataSet Implements IDepository.GetDataForGSOConformityReport

        Dim dstResults As New DataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForGSOConformityReport(p_strBatchNumber, p_intCertificationTypeId, p_intTireTypeId)
        End Using
        Return dstResults

    End Function


    ''' <summary>
    ''' Gets the data for Imark report.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForImarkConformityReport() As DataSet Implements IDepository.GetDataForImarkConformityReport

        Dim dstResults As New DataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForIMARKConformityReport()
        End Using
        Return dstResults

    End Function

    ''' <summary>
    ''' Get Imark Sampling Report data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataForImarkSamplingTireTestsReport(ByVal p_strMatlNum As String) As DataSet Implements IDepository.GetDataForImarkSamplingTireTestsReport

        Dim dstResults As New DataSet
        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetDataForImarkSamplingTireTestsReport(p_strMatlNum)
        End Using
        Return dstResults

    End Function

    ' Changed as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation 
    Public Function GetDataForSKUCertification(ByVal p_strMatlNum As String, ByVal p_strBrand As String, ByVal p_strBrandLine As String, ByVal p_strCertType As String) As CertificateReport Implements IDepository.GetDataForSKUCertification

        Dim dstResults As New CertificateReport
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetSKUCertificateReportInfo(p_strMatlNum, p_strBrand, p_strBrandLine, p_strCertType)
        End Using
        Return dstResults

    End Function

    Public Function GetDataForImarkCertification(ByVal p_dtDateParam As DateTime) As ImarkCertificationDataSet Implements IDepository.GetDataForImarkCertification

        Dim dstResults As New ImarkCertificationDataSet
        Using ctdReportsDalc As ReportsDalc = New ReportsDalc
            dstResults = ctdReportsDalc.GetDataForImarkCertification(p_dtDateParam)
        End Using
        Return dstResults

    End Function

    'JBH_2.00 Project 5325 - App Mold Change Required and Operations Approval Date Parameters
    'JESEITZ 10/292016 - REQ 203625 AddInfo field
    Public Function SaveCertificate(ByVal p_iSKUId As Integer, ByVal p_strMatlNum As String, ByVal p_blnRemoveMatlNum As Boolean, ByVal p_strCertificationTypeName As String, ByVal p_strCERTIFICATENUMBER As String, ByVal p_dteCertDateSubmitted As Date, ByVal p_dteCertDateApproved_CEGI As Date, ByVal p_dteDATESUBMITED As Date, ByVal pc_ACTIVESTATUS As String, ByVal p_dteDATEASSIGNED_EGI As Date, ByVal p_dteDATEAPROVED_CEGI As Date, ByVal pc_RENEWALREQUIRED_CGIN As Char, ByVal p_strJOBREPORTNUMBER_CEN As String, ByVal p_strEXTENSION_EN As String, ByVal p_strSUPPLEMENTALMOLDSTAMPING_E As String, ByVal p_strEMARKREFERENCE_I As String, ByVal p_dteEXPIRYDATE_I As Date, ByVal p_strFAMILY_I As String, ByVal p_strPRODUCTLOCATION As String, ByVal p_strCOUNTRYOFMANUFACTURE_N As String, ByVal p_blnAddNewCustomer As Boolean, ByVal p_strActSigReq As String, ByVal p_intCustomerId As Integer, ByVal p_strCUSTOMER_N As String, ByVal p_strCustomerAddress As String, ByVal p_strCUSTOMERSPECIFIC_N As String, ByVal p_blnAddNewImporter As Boolean, ByVal p_intImporterId As Integer, ByVal p_strImporter As String, ByVal p_strImporterAddress As String, ByVal p_strImporterRepresentative As String, ByVal p_strCOUNTRYLOCATION_N As String, ByVal p_strBATCHNUMBER_G As String, ByVal p_dteSUPPLEMENTALASSIGNED As Date, ByVal p_dteSUPPLEMENTALSUBMITTED As Date, ByVal p_dteSUPPLEMENTALAPPROVED As Date, ByVal p_strCompanyName As String, ByVal p_strUserName As String, ByRef p_intCertificateNumberID As Integer, ByVal p_strFamilyDesc As String, ByVal p_blnMoldChgRequired As Boolean, ByVal p_dteOperDateApproved As DateTime, ByVal p_strAddInfo As String) As Common.NameAid.SaveResult Implements IDepository.SaveCertificate
        Dim enuSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enuSaveResult = ctdCertDalc.SaveCertificate(p_iSKUId, p_strMatlNum, p_blnRemoveMatlNum, p_strCertificationTypeName, p_strCERTIFICATENUMBER, p_dteCertDateSubmitted, p_dteCertDateApproved_CEGI, p_dteDATESUBMITED, pc_ACTIVESTATUS, p_dteDATEASSIGNED_EGI, p_dteDATEAPROVED_CEGI, pc_RENEWALREQUIRED_CGIN, p_strJOBREPORTNUMBER_CEN, p_strEXTENSION_EN, p_strSUPPLEMENTALMOLDSTAMPING_E, p_strEMARKREFERENCE_I, p_dteEXPIRYDATE_I, p_strFAMILY_I, p_strPRODUCTLOCATION, p_strCOUNTRYOFMANUFACTURE_N, p_blnAddNewCustomer, p_strActSigReq, p_intCustomerId, p_strCUSTOMER_N, p_strCustomerAddress, p_strCUSTOMERSPECIFIC_N, p_blnAddNewImporter, p_intImporterId, p_strImporter, p_strImporterAddress, p_strImporterRepresentative, p_strCOUNTRYLOCATION_N, p_strBATCHNUMBER_G, p_dteSUPPLEMENTALASSIGNED, p_dteSUPPLEMENTALSUBMITTED, p_dteSUPPLEMENTALAPPROVED, p_strCompanyName, p_strUserName, p_intCertificateNumberID, p_strFamilyDesc, p_blnMoldChgRequired, p_dteOperDateApproved, p_strAddInfo)
        End Using
        Return enuSaveResult

    End Function

    Public Function BatchNumMassUpdate(ByVal p_strCertifName As String, ByVal p_strTempBatchNum As String, ByVal p_strGSOBatchNum As String, ByVal p_strUserName As String) As Common.NameAid.SaveResult Implements IDepository.BatchNumMassUpdate

        Dim enuSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enuSaveResult = ctdCertDalc.BatchNumMassUpdate(p_strCertifName, p_strTempBatchNum, p_strGSOBatchNum, p_strUserName)
        End Using
        Return enuSaveResult

    End Function

    ' Changed as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation 
    ''' <summary>
    ''' Gets the data for Emark report.
    ''' </summary>
    ''' <returns>EmarkCertificationTypeDataset</returns>
    Public Function GetDataForEmarkCertificationReport(ByVal p_strCertificationNo, ByVal p_strBrand, ByVal p_strBrandLine) As DataSet Implements IDepository.GetDataForEmarkCertificationReport
        Dim dstResults As New DataSet

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetDataForEmarkCertificationReport(p_strCertificationNo, p_strBrand, p_strBrandLine)
        End Using

        Return dstResults
    End Function

    ''' <summary>
    ''' Get traceability report info
    ''' </summary>
    ''' <param name="p_strCertificateNumber"></param>
    ''' <param name="p_intCertificationTypeID"></param>
    ''' <param name="p_strIncludeArchived">Include Archived certificates.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTraceabilityReportInfo(ByVal p_strCertificateNumber As String, ByVal p_intCertificationTypeID As Integer, ByVal p_strIncludeArchived As String) As Traceability Implements IDepository.GetTraceabilityReportInfo

        Dim dstResults As New Traceability

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetTraceabilityReportInfo(p_strCertificateNumber, p_intCertificationTypeID, p_strIncludeArchived)
        End Using

        Return dstResults

    End Function

    ''' <summary>
    ''' Get Exception report info data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetExceptionReportInfo() As ExceptionReport_DataSet Implements IDepository.GetExceptionReportInfo

        Dim dstResults As New ExceptionReport_DataSet

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetExceptionReportInfo()
        End Using

        Return dstResults

    End Function

    ''' Get Emark Application report info data
    Public Function GetEmarkApplication(ByVal p_strCertificateNumber As String, _
                                                 ByVal p_intTireTypeID As Integer) As DataSet Implements IDepository.GetEmarkApplication

        Dim dstResults As New DataSet
        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetEmarkApplication(p_strCertificateNumber, p_intTireTypeID)
        End Using
        Return dstResults

    End Function

    ''' Get Nom Certification report info data
    Public Function GetNomCertification(ByVal p_strCertificateNumber As String, ByVal p_intTireTypeID As Integer) As DataSet Implements IDepository.GetNomCertification

        Dim dstResults As New DataSet

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetNomCertification(p_strCertificateNumber, p_intTireTypeID)
        End Using

        Return dstResults

    End Function

    ''' Get Imark Authenticity report info data
    Public Function GetAuthenticity() As DataSet Implements IDepository.GetAuthenticityReport

        Dim dstResults As New DataSet

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetAuthenticityReport
        End Using

        Return dstResults

    End Function

    ''' Get EmarkSimilarCertificateSearchReport data
    Public Function GetEmarkSimilarCertificateSearchReport(ByVal p_MatlNum As String) As DataSet Implements IDepository.GetEmarkSimilarCertificateSearchReport

        Dim dstResults As New DataSet

        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetEmarkSimilarCertificateSearchReport(p_MatlNum)
        End Using

        Return dstResults

    End Function

    ''' Get Certification Renewal report info data
    Public Function GetCertificationRenewal(ByVal p_strCertificateNumber As String, ByVal p_intCertificationTypeID As Integer) As DataSet Implements IDepository.GetCertificationRenewal

        Dim dstResults As New DataSet
        Using crtRptDal As ReportsDalc = New ReportsDalc
            dstResults = crtRptDal.GetCertificationRenewal(p_strCertificateNumber, p_intCertificationTypeID)
        End Using
        Return dstResults

    End Function

    Public Function SaveAuditLogEntry(ByVal p_dteChangeDateTime As Date, ByVal m_strChangedBy As String, ByVal m_strArea As String, ByVal m_strChangedFieldElement As String, ByVal m_strOldValue As String, ByVal m_strNewValue As String, ByVal m_intReasonID As Integer, ByVal m_strNote As String) As Boolean Implements IDepository.SaveAuditLogEntry

        Dim blnSaved As Boolean = False
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnSaved = ctdCertDalc.SaveAuditLogEntry(p_dteChangeDateTime, m_strChangedBy, m_strArea, m_strChangedFieldElement, m_strOldValue, m_strNewValue, m_intReasonID, m_strNote)
        End Using
        Return blnSaved

    End Function

    Function GetProductCertStatus(ByVal p_strBrand As String, ByVal p_strBrandLine As String) As DataSet Implements IDepository.GetProductCertStatus
        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetProductCertStatus(p_strBrand, p_strBrandLine)
        End Using

        Return dstResults

    End Function


    Public Function GetRegionCertStatus(ByVal p_strBrand As String, ByVal p_strBrandLine As String) As DataSet Implements IDepository.GetRegionCertStatus

        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetRegionCertStatus(p_strBrand, p_strBrandLine)
        End Using

        Return dstResults

    End Function

    Public Function GetCountries(ByVal p_strRegionName As String) As DataTable Implements IDepository.GetCountries

        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetCountries(p_strRegionName)
        End Using

        Return dstResults.Tables(0)

    End Function

    Public Function GetImporters() As DataTable Implements IDepository.GetImporters

        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetImporters()
        End Using

        Return dstResults.Tables(0)

    End Function

    Public Function GetCustomers() As DataTable Implements IDepository.GetCustomers

        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetCustomers()
        End Using

        Return dstResults.Tables(0)

    End Function

    Public Function SaveCertificationRequest(ByVal p_blnDeleteMe As Boolean, ByVal p_strMatlNum As String, ByVal p_intCountryID As Integer, ByVal p_intSKUID As Integer) As Boolean Implements IDepository.SaveCertificationRequest
        ' old marketing screen
        Dim blnDone As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.SaveCertificationRequest(p_blnDeleteMe, p_strMatlNum, p_intCountryID, p_intSKUID)
        End Using

        Return blnDone

    End Function

    Public Function SaveCertificationGroup(ByVal p_blnDeleteMe As Boolean, ByVal p_strMatlNum As String, ByVal p_intCertificationID As Integer, ByVal p_intSKUID As Integer) As Boolean Implements IDepository.SaveCertificationGroup
        ' old marketing screen
        Dim blnDone As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.SaveCertificationGroup(p_blnDeleteMe, p_strMatlNum, p_intCertificationID, p_intSKUID)
        End Using

        Return blnDone

    End Function

    Public Function SaveRequestCert(ByVal p_blnDeleteMe As Boolean, ByVal p_strMatlNum As String, ByVal p_intCertificationID As Integer, ByVal p_intSKUID As Integer) As Boolean Implements IDepository.SaveRequestCert
        ' jeseitz 6/2/16 - added for MarketingNew screen
        Dim blnDone As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.SaveRequestCert(p_blnDeleteMe, p_strMatlNum, p_intCertificationID, p_intSKUID)
        End Using

        Return blnDone

    End Function

    Public Function CheckMatlNumExists(ByVal p_strMatlNum As String) As Boolean Implements IDepository.CheckMatlNumExists

        Dim blnExists As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnExists = ctdCertDalc.CheckIfMatlNumExists(p_strMatlNum)
        End Using

        Return blnExists

    End Function

    Public Function CheckIfCertificateNumberExists(ByVal p_strCertNum As String) As Boolean Implements IDepository.CheckIfCertificateNumberExists

        Dim blnExists As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnExists = ctdCertDalc.CheckIfCertificateNumberExists(p_strCertNum)
        End Using

        Return blnExists

    End Function

    Public Function RenewCertificate(ByVal p_intCertificateId As Integer, ByRef p_intNewCertificateID As Integer, ByVal p_strUserName As String) As NameAid.SaveResult Implements IDepository.RenewCertificate

        Dim enumSaveResult As NameAid.SaveResult

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.RenewCertificate(p_intCertificateId, p_intNewCertificateID, p_strUserName)
        End Using

        Return enumSaveResult

    End Function

    Public Function GetLatestImarkCertifId() As Integer Implements IDepository.GetLatestImarkCertifId

        Dim strImarkCertId As Integer

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strImarkCertId = ctdCertDalc.GetLatestImarkCertifId()
        End Using

        Return strImarkCertId

    End Function

    Public Function GetCertifExtension(ByVal p_intImarkCertId As Integer) As String Implements IDepository.GetCertifExtension

        Dim strImarkCertExtension As String = String.Empty

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strImarkCertExtension = ctdCertDalc.GetCertifExtension(p_intImarkCertId)
        End Using

        Return strImarkCertExtension

    End Function

    Public Function GetLatestGSOCertifNumber() As String Implements IDepository.GetLatestGSOCertifNumber

        Dim strGSOTempCertNumber As String = String.Empty

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strGSOTempCertNumber = ctdCertDalc.GetLatestGSOCertifNumber()
        End Using

        Return strGSOTempCertNumber

    End Function

    Public Function SaveNewCertificate(ByVal p_strCertNum As String, ByVal p_intCertTypeId As Integer, ByVal p_strMatlNum As String, ByVal p_strImporter As String, ByVal p_strCustomer As String, ByVal p_strUserName As String, ByVal p_strExtension As String) As Boolean Implements IDepository.SaveNewCertificate

        Dim blnDone As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.SaveNewCertificate(p_strCertNum, p_intCertTypeId, p_strMatlNum, p_strImporter, p_strCustomer, p_strUserName, p_strExtension)
        End Using

        Return blnDone

    End Function

    Public Function SaveNewCertificate(ByVal p_strCertNum As String, ByVal p_intCertTypeId As Integer, ByVal p_strMatlNum As String, ByVal p_strImporter As String, ByVal p_strCustomer As String, ByVal p_strUserName As String, ByVal p_strExtension As String, ByVal p_InsertPC As String, ByRef p_ErrorDesc As String) As Integer Implements IDepository.SaveNewCertificate

        Dim resultNum As Integer

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            resultNum = ctdCertDalc.SaveNewCertificate(p_strCertNum, p_intCertTypeId, p_strMatlNum, p_strImporter, p_strCustomer, p_strUserName, p_strExtension, p_InsertPC, p_ErrorDesc)
        End Using

        Return resultNum

    End Function

    Public Function ArchiveCertification(ByVal p_strCertNum As String, ByVal p_strUserName As String) As Boolean Implements IDepository.ArchiveCertification

        Dim blnDone As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.ArchiveCertification(p_strCertNum, p_strUserName)
        End Using

        Return blnDone

    End Function

    Public Function GetCertifications() As DataSet Implements IDepository.GetCertifications
        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetCertifications()
        End Using

        Return dstResults
    End Function

    Public Function GetSearchTypeResults() As DataSet Implements IDepository.GetSearchTypeResults
        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetSearchTypeResults()
        End Using

        Return dstResults
    End Function

    Public Function GetManufacturingLocationsResults(ByVal p_strSize As String) As DataSet Implements IDepository.GetManufacturingLocationsResults
        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetManufacturingLocationsResults(p_strSize)
        End Using

        Return dstResults
    End Function

    Public Function GetCompanyNameList() As DataSet Implements IDepository.GetCompanyNameList

        Dim dstResults As New DataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetCompanyNameList()
        End Using

        Return dstResults
    End Function

    Public Function GetCertificationSearchResults(ByVal p_strSearchCriteria As String, ByVal p_strSearchType As String, ByVal p_strExtensionNo As String, ByVal p_strImarkFamily As String, ByVal ps_BrandLine As String) As DataTable Implements IDepository.GetCertificationSearchResults
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetCertificationSearchResults(p_strSearchCriteria, p_strSearchType, p_strExtensionNo, p_strImarkFamily, ps_BrandLine)
        End Using

        Return dtbResults
    End Function

    ' Added GetBrands function as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation 
    Public Function GetBrands(ByVal p_strBrand As String) As DataTable Implements IDepository.GetBrands
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetBrands(p_strBrand)
        End Using

        Return dtbResults
    End Function

    Public Function GetBrandLines(ByVal p_strBrandLine As String) As DataTable Implements IDepository.GetBrandLines
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetBrandLines(p_strBrandLine)
        End Using

        Return dtbResults
    End Function

    Function GetMaterialAttribs(ByVal p_strMatlNum As String) As DataTable Implements IDepository.GetMaterialAttribs
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetMaterialAttribs(p_strMatlNum)
        End Using

        Return dtbResults
    End Function

    ''' <summary>
    ''' Get certificate data from database
    ''' </summary>
    ''' <param name="ps_CertificationNumber"></param>
    ''' <param name="ps_ExtensionNo"></param>
    ''' <param name="ps_CertificationTypeID"></param>
    ''' <param name="p_iSKUID"></param>
    ''' <param name="p_blnTRsExist"></param>
    ''' <returns>dtbResults</returns>
    ''' <remarks></remarks>

    Public Function GetCertificate(ByVal ps_CertificationNumber As String, ByVal ps_ExtensionNo As String, ByVal ps_CertificationTypeID As Integer, ByVal p_iSKUID As Integer, ByRef p_blnTRsExist As Boolean) As DataTable Implements IDepository.GetCertificate
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetCertificate(ps_CertificationNumber, ps_ExtensionNo, ps_CertificationTypeID, p_iSKUID, p_blnTRsExist)
        End Using

        Return dtbResults
    End Function

    Public Function GetSimilarCertificate(ByVal p_iCertificationTypeID As Integer, ByVal p_strMatlNum As String, ByVal p_strCertificationNumber As String) As DataTable Implements IDepository.GetSimilarCertificate
        Dim dtbResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetSimilarCertificate(p_iCertificationTypeID, p_strMatlNum, p_strCertificationNumber)
        End Using

        Return dtbResults
    End Function

    Public Function GetProductData(ByVal p_strMatlNum As String, ByVal p_iSKUID As Integer) As ICSDataSet.ProductDataDataTable Implements IDepository.GetProductData
        Dim dtbResult As New ICSDataSet.ProductDataDataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResult = ctdCertDalc.GetProductData(p_strMatlNum, p_iSKUID)
        End Using

        Return dtbResult
    End Function

    Public Function GetCertificatesByType(ByVal p_certificationtypeid As Integer, ByVal p_all As String) As DataTable Implements IDepository.GetCertificatesByType
        Dim dtbResult As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResult = ctdCertDalc.GetCertificatesByType(p_certificationtypeid, p_all)
        End Using

        Return dtbResult
    End Function

    Public Function GetTestResultData(ByVal p_strMatlNum As String, ByVal p_intSKUID As Integer, ByVal p_strCertificateNumber As String, ByVal p_intCertificateNumberID As Integer, ByVal p_intCertificationTypeId As Integer) As ICSDataSet Implements IDepository.GetTestResultData
        Dim dstResults As New ICSDataSet

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dstResults = ctdCertDalc.GetTestResultData(p_strMatlNum, p_intSKUID, p_strCertificateNumber, p_intCertificateNumberID, p_intCertificationTypeId)
        End Using

        Return dstResults
    End Function

    Public Function Save_Product(ByVal p_iSKUID As Integer, _
                                 ByVal p_strMatlNum As String, _
                                 ByVal p_strBrand As String, _
                                 ByVal p_strBrandLine As String, _
                                 ByVal p_iTireTypeId As Integer, _
                                 ByVal p_strPSN As String, _
                                 ByVal p_strSizeStamp As String, _
                                 ByVal p_dteDiscontinuedDate As DateTime, _
                                 ByVal p_strSPECNUMBER As String, _
                                 ByVal p_strSPEEDRATING As String, _
                                 ByVal p_strSINGLOADINDEX As String, _
                                 ByVal p_strDUALLOADINDEX As String, _
                                 ByVal p_strBELTEDRADIALYN As String, _
                                 ByVal p_strTUBElESSYN As String, _
                                 ByVal p_strREINFORCEDYN As String, _
                                 ByVal p_strEXTRALOADYN As String, _
                                 ByVal p_strUTQGTREADWEAR As String, _
                                 ByVal p_strUTQGTRACTION As String, _
                                 ByVal p_strUTQGTEMP As String, _
                                 ByVal p_strMUDSNOWYN As String, _
                                 ByVal p_iRIMDIAMETER As Single, _
                                 ByVal p_dteSerialDate As DateTime, _
                                 ByVal p_strBrandDesc As String, _
                                 ByVal p_strMeaRimWidth As Single, _
                                 ByVal p_strLoadRange As String, _
                                 ByVal p_strRegroovableInd As String, _
                                 ByVal p_strPlantProduced As String, _
                                 ByVal p_dteMostRecentTestDate As DateTime, _
                                 ByVal p_strIMark As String, _
                                 ByVal p_strInformeNumber As String, _
                                 ByVal p_dteFechaDate As DateTime, _
                                 ByVal p_strTreadPattern As String, _
                                 ByVal p_strSpecialProtectiveBand As String, _
                                 ByVal p_strNominalTireWidth As String, _
                                 ByVal p_strAspectRadio As String, _
                                 ByVal p_strTreadwearIndicators As String, _
                                 ByVal p_strNameOfManufacturer As String, _
                                 ByVal p_strFamily As String, _
                                 ByVal p_strDOTSerialNumber As String, _
                                 ByVal p_strTPN As String, _
                                 ByVal p_strUserName As String, _
                                 ByVal p_strSEVEREWEATHERIND As String, _
                                 ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.Save_Product
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.Save_Product(p_iSKUID, _
                                                      p_strMatlNum, _
                                                      p_strBrand, _
                                                       p_strBrandLine, _
                                                      p_iTireTypeId, _
                                                      p_strPSN, _
                                                      p_strSizeStamp, _
                                                      p_dteDiscontinuedDate, _
                                                      p_strSPECNUMBER, _
                                                      p_strSPEEDRATING, _
                                                      p_strSINGLOADINDEX, _
                                                      p_strDUALLOADINDEX, _
                                                      p_strBELTEDRADIALYN, _
                                                      p_strTUBElESSYN, _
                                                      p_strREINFORCEDYN, _
                                                      p_strEXTRALOADYN, _
                                                      p_strUTQGTREADWEAR, _
                                                      p_strUTQGTRACTION, _
                                                      p_strUTQGTEMP, _
                                                      p_strMUDSNOWYN, _
                                                      p_iRIMDIAMETER, _
                                                      p_dteSerialDate, _
                                                      p_strBrandDesc, _
                                                      p_strMeaRimWidth, _
                                                      p_strLoadRange, _
                                                      p_strRegroovableInd, _
                                                      p_strPlantProduced, _
                                                      p_dteMostRecentTestDate, _
                                                      p_strIMark, _
                                                      p_strInformeNumber, _
                                                      p_dteFechaDate, _
                                                      p_strTreadPattern, _
                                                      p_strSpecialProtectiveBand, _
                                                      p_strNominalTireWidth, _
                                                      p_strAspectRadio, _
                                                      p_strTreadwearIndicators, _
                                                      p_strNameOfManufacturer, _
                                                      p_strFamily, _
                                                      p_strDOTSerialNumber, _
                                                      p_strTPN, _
                                                      p_strUserName, _
                                                      p_strSEVEREWEATHERIND, _
                                                      p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveMeasurement(ByVal p_strPROJECTNUMBER As String, _
                                    ByVal p_sngTIRENUMBER As Single, _
                                    ByVal p_strTESTSPEC As String, _
                                    ByVal p_dteCOMPLETIONDATE As DateTime, _
                                    ByVal p_sngINFLATIONPRESSURE As Single, _
                                    ByVal p_strMOLDDESIGN As String, _
                                    ByVal p_sngRIMWIDTH As Single, _
                                    ByVal p_strDOTSERIALNUMBER As String, _
                                    ByVal p_sngDIAMETER As Single, _
                                    ByVal p_sngAVGSECTIONWIDTH As Single, _
                                    ByVal p_sngAVGOVERALLWIDTH As Single, _
                                    ByVal p_sngMAXOVERALLWIDTH As Single, _
                                    ByVal p_sngSIZEFACTOR As Single, _
                                    ByVal p_dteMOUNTTIME As DateTime, _
                                    ByVal p_sngMOUNTTEMP As Single, _
                                    ByVal p_intSKUID As Integer, _
                                    ByVal p_intCertType As Integer, _
                                    ByVal p_strCERTIFICATENUMBER As String, _
                                    ByRef p_intMEASUREID As Integer, _
                                    ByVal p_dteSerialDate As DateTime, _
                                    ByVal p_dteEndTime As DateTime, _
                                    ByVal p_sngActSizeFactor As Single, _
                                    ByVal p_srtSTARTINFLATIONPRESSURE As Short, _
                                    ByVal p_srtENDINFLATIONPRESSURE As Short, _
                                    ByVal p_strADJUSTMENT As String, _
                                    ByVal p_sngCIRCUNFERENCE As Single, _
                                    ByVal p_sngNOMINALDIAMETER As Single, _
                                    ByVal p_sngNOMINALWIDTH As Single, _
                                    ByVal p_strNOMINALWIDTHPASSFAIL As String, _
                                    ByVal p_sngNOMINALWIDTHDIFERENCE As Single, _
                                    ByVal p_sngNOMINALWIDTHTOLERANCE As Single, _
                                    ByVal p_sngMAXOVERALLDIAMETER As Single, _
                                    ByVal p_sngMINOVERALLDIAMETER As Single, _
                                    ByVal p_strOVERALLWIDTHPASSFAIL As String, _
                                    ByVal p_strOVERALLDIAMETERPASSFAIL As String, _
                                    ByVal p_sngDIAMETERDIFERENCE As Single, _
                                    ByVal p_sngDIAMETERTOLERANCE As Single, _
                                    ByVal p_strTEMPRESISTANCEGRADING As String, _
                                    ByVal p_sngTENSILESTRENGHT1 As Single, _
                                    ByVal p_sngTENSILESTRENGHT2 As Single, _
                                    ByVal p_sngELONGATION1 As Single, _
                                    ByVal p_sngELONGATION2 As Single, _
                                    ByVal p_sngTENSILESTRENGHTAFTERAGE1 As Single, _
                                    ByVal p_sngTENSILESTRENGHTAFTERAGE2 As Single, _
                                    ByVal p_strOperatorName As String, _
                                    ByVal p_intCertificateID As Integer, _
                                    ByVal p_strMatlNum As String, _
                                    ByVal p_strOperation As String, _
                                    ByVal p_strGTSpecMeasurement As String, _
                                    ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveMeasurement
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveMeasurement(p_strPROJECTNUMBER, _
                                                         p_sngTIRENUMBER, _
                                                         p_strTESTSPEC, _
                                                         p_dteCOMPLETIONDATE, _
                                                         p_sngINFLATIONPRESSURE, _
                                                         p_strMOLDDESIGN, _
                                                         p_sngRIMWIDTH, _
                                                         p_strDOTSERIALNUMBER, _
                                                         p_sngDIAMETER, _
                                                         p_sngAVGSECTIONWIDTH, _
                                                         p_sngAVGOVERALLWIDTH, _
                                                         p_sngMAXOVERALLWIDTH, _
                                                         p_sngSIZEFACTOR, _
                                                         p_dteMOUNTTIME, _
                                                         p_sngMOUNTTEMP, _
                                                         p_intSKUID, _
                                                         p_intCertType, _
                                                         p_strCERTIFICATENUMBER, _
                                                         p_intMEASUREID, _
                                                         p_dteSerialDate, _
                                                         p_dteEndTime, _
                                                         p_sngActSizeFactor, _
                                                         p_srtSTARTINFLATIONPRESSURE, _
                                                         p_srtENDINFLATIONPRESSURE, _
                                                         p_strADJUSTMENT, _
                                                         p_sngCIRCUNFERENCE, _
                                                         p_sngNOMINALDIAMETER, _
                                                         p_sngNOMINALWIDTH, _
                                                         p_strNOMINALWIDTHPASSFAIL, _
                                                         p_sngNOMINALWIDTHDIFERENCE, _
                                                         p_sngNOMINALWIDTHTOLERANCE, _
                                                         p_sngMAXOVERALLDIAMETER, _
                                                         p_sngMINOVERALLDIAMETER, _
                                                         p_strOVERALLWIDTHPASSFAIL, _
                                                         p_strOVERALLDIAMETERPASSFAIL, _
                                                         p_sngDIAMETERDIFERENCE, _
                                                         p_sngDIAMETERTOLERANCE, _
                                                         p_strTEMPRESISTANCEGRADING, _
                                                         p_sngTENSILESTRENGHT1, _
                                                         p_sngTENSILESTRENGHT2, _
                                                         p_sngELONGATION1, _
                                                         p_sngELONGATION2, _
                                                         p_sngTENSILESTRENGHTAFTERAGE1, _
                                                         p_sngTENSILESTRENGHTAFTERAGE2, _
                                                         p_strOperatorName, _
                                                         p_intCertificateID, _
                                                         p_strMatlNum, _
                                                         p_strOperation, _
                                                         p_strGTSpecMeasurement, _
                                                         p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function MeasurementDetail_Save(ByVal p_sngSectionWidth As Single, _
                                           ByVal p_sngOVERALLWIDTH As Single, _
                                           ByVal p_iMEASUREID As Integer, _
                                           ByVal p_sngITERATION As Single, _
                                           ByVal p_strUserName As String) As NameAid.SaveResult Implements IDepository.MeasurementDetail_Save
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.MeasurementDetail_Save(p_sngSectionWidth, _
                                                                p_sngOVERALLWIDTH, _
                                                                p_iMEASUREID, _
                                                                p_sngITERATION, _
                                                                p_strUserName)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SavePlunger(ByVal p_strPROJECTNUMBER As String, _
                                ByVal p_sngTIRENUMBER As Single, _
                                ByVal p_strTESTSPEC As String, _
                                ByVal p_dteCOMPLETIONDATE As DateTime, _
                                ByVal p_strDOTSERIALNUMBER As String, _
                                ByVal p_sngAVGBREAKINGENERGY As Single, _
                                ByVal p_strPASSYN As String, _
                                ByVal p_intSKUID As Integer, _
                                ByVal p_intCertType As Integer, _
                                ByVal p_strCERTIFICATENUMBER As String, _
                                ByRef p_intPLUNGERID As Integer, _
                                ByVal p_dteSerialDate As DateTime, _
                                ByVal p_sngMinPlunger As Single, _
                                ByVal p_strUserName As String, _
                                ByVal p_intCertificateNumberID As Integer, _
                                ByVal p_strMatlNum As String, _
                                ByVal p_strOperation As String, _
                                ByVal p_strGTSpecPlunger As String, _
                                ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SavePlunger
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SavePlunger(p_strPROJECTNUMBER, _
                                                     p_sngTIRENUMBER, _
                                                     p_strTESTSPEC, _
                                                     p_dteCOMPLETIONDATE, _
                                                     p_strDOTSERIALNUMBER, _
                                                     p_sngAVGBREAKINGENERGY, _
                                                     p_strPASSYN, _
                                                     p_intSKUID, _
                                                     p_intCertType, _
                                                     p_strCERTIFICATENUMBER, _
                                                     p_intPLUNGERID, _
                                                     p_dteSerialDate, _
                                                     p_sngMinPlunger, _
                                                     p_strUserName, _
                                                     p_intCertificateNumberID, _
                                                     p_strMatlNum, _
                                                     p_strOperation, _
                                                     p_strGTSpecPlunger, _
                                                     p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SavePlungerDetail(ByVal p_sngBREAKINGENERGY As Single, _
                                      ByVal p_intPlungerID As Integer, _
                                      ByVal p_sngIteration As Single, _
                                      ByVal p_strUserName As String) As NameAid.SaveResult Implements IDepository.SavePlungerDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SavePlungerDetail(p_sngBREAKINGENERGY, _
                                                           p_intPlungerID, _
                                                           p_sngIteration, _
                                                           p_strUserName)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveTreadWear(ByVal p_strPROJECTNUMBER As String, _
                                  ByVal p_sngTIRENUMBER As Single, _
                                  ByVal p_strTESTSPEC As String, _
                                  ByVal p_dteCOMPLETIONDATE As DateTime, _
                                  ByVal p_strDOTSERIALNUMBER As String, _
                                  ByVal p_sngLOWESTWEARBAR As Single, _
                                  ByVal p_strPassyn As String, _
                                  ByVal p_intSKUID As Integer, _
                                  ByVal p_intCertType As Integer, _
                                  ByVal p_strCERTIFICATENUMBER As String, _
                                  ByRef p_intTREADWEARID As Integer, _
                                  ByVal p_dteSERIALDATE As DateTime, _
                                  ByVal p_strOperatorName As String, _
                                  ByVal p_sngINDICATORSREQUIREMENT As Single, _
                                  ByVal p_intCertificateID As Integer, _
                                  ByVal p_strMatlNum As String, _
                                  ByVal p_strOperation As String, _
                                  ByVal p_strGTSpecTreadWear As String, _
                                  ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveTreadWear
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveTreadWear(p_strPROJECTNUMBER, _
                                                       p_sngTIRENUMBER, _
                                                       p_strTESTSPEC, _
                                                       p_dteCOMPLETIONDATE, _
                                                       p_strDOTSERIALNUMBER, _
                                                       p_sngLOWESTWEARBAR, _
                                                       p_strPassyn, _
                                                       p_intSKUID, _
                                                       p_intCertType, _
                                                       p_strCERTIFICATENUMBER, _
                                                       p_intTREADWEARID, _
                                                       p_dteSERIALDATE, _
                                                       p_strOperatorName, _
                                                       p_sngINDICATORSREQUIREMENT, _
                                                       p_intCertificateID, _
                                                       p_strMatlNum, _
                                                       p_strOperation, _
                                                       p_strGTSpecTreadWear, _
                                                       p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveTreadWearDetail(ByVal p_sngwearbarheight As Single, _
                                        ByVal p_intTREADWEARID As Integer, _
                                        ByVal p_sngIteration As Single, _
                                        ByVal p_strOperatorName As String) As NameAid.SaveResult Implements IDepository.SaveTreadWearDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveTreadWearDetail(p_sngwearbarheight, _
                                                             p_intTREADWEARID, _
                                                             p_sngIteration, _
                                                             p_strOperatorName)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveBeadUnseat(ByVal p_strPROJECTNUMBER As String, _
                                   ByVal p_sngTIRENUMBER As Single, _
                                   ByVal p_strTESTSPEC As String, _
                                   ByVal p_dteCOMPLETIONDATE As DateTime, _
                                   ByVal p_strDOTSERIALNUMBER As String, _
                                   ByVal p_sngLOWESTUNSEATVALUE As Single, _
                                   ByVal p_strPassyn As String, _
                                   ByVal p_intCertType As Integer, _
                                   ByVal p_strCERTIFICATENUMBER As String, _
                                   ByRef p_intBeadUnseatID As Integer, _
                                   ByVal p_dteSerialDate As DateTime, _
                                   ByVal p_sngMINBEADUNSEAT As Single, _
                                   ByVal p_strTESTPASSFAIL As String, _
                                   ByVal p_strOperatorName As String, _
                                   ByVal p_intCertificateID As Integer, _
                                   ByVal p_strMatlNum As String, _
                                   ByVal p_strOperation As String, _
                                   ByVal p_strGTSpecBeadUnseat As String, _
                                   ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveBeadUnseat
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveBeadUnseat(p_strPROJECTNUMBER, _
                                                        p_sngTIRENUMBER, _
                                                        p_strTESTSPEC, _
                                                        p_dteCOMPLETIONDATE, _
                                                        p_strDOTSERIALNUMBER, _
                                                        p_sngLOWESTUNSEATVALUE, _
                                                        p_strPassyn, _
                                                        p_intCertType, _
                                                        p_strCERTIFICATENUMBER, _
                                                        p_intBeadUnseatID, _
                                                        p_dteSerialDate, _
                                                        p_sngMINBEADUNSEAT, _
                                                        p_strTESTPASSFAIL, _
                                                        p_strOperatorName, _
                                                        p_intCertificateID, _
                                                        p_strMatlNum, _
                                                        p_strOperation, _
                                                        p_strGTSpecBeadUnseat, _
                                                        p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveBeadUnseatDetail(ByVal p_intBEADUNSEATID As Integer, _
                                         ByVal p_sngUNSEATFORCE As Single, _
                                         ByVal p_sngIteration As Single, _
                                         ByVal p_strOperatorName As String) As NameAid.SaveResult Implements IDepository.SaveBeadUnseatDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveBeadUnseatDetail(p_intBEADUNSEATID, _
                                                              p_sngUNSEATFORCE, _
                                                              p_sngIteration, _
                                                              p_strOperatorName)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveEndurance(ByRef p_intENDURANCEID As Integer, _
                                  ByVal p_strProjectNumber As String, _
                                  ByVal p_intTireNumber As Integer, _
                                  ByVal p_strTESTSPEC As String, _
                                  ByVal p_dteCOMPLETIONDATE As DateTime, _
                                  ByVal p_strDOTSERIALNUMBER As String, _
                                  ByVal p_dtePRECONDSTARTDATE As DateTime, _
                                  ByVal p_sngPRECONDSTARTTEMP As Single, _
                                  ByVal p_sngRIMDIAMETER As Single, _
                                  ByVal p_sngRIMWIDTH As Single, _
                                  ByVal p_dtePRECONDENDDATE As DateTime, _
                                  ByVal p_intPRECONDENDTEMP As Integer, _
                                  ByVal p_intINFLATIONPRESSURE As Integer, _
                                  ByVal p_sngBEFOREDIAMETER As Single, _
                                  ByVal p_sngAFTERDIAMETER As Single, _
                                  ByVal p_intBEFOREINFLATION As Integer, _
                                  ByVal p_intAFTERINFLATION As Integer, _
                                  ByVal p_intWHEELPOSITION As Integer, _
                                  ByVal p_intWHEELNUMBER As Integer, _
                                  ByVal p_intFINALTEMP As Integer, _
                                  ByVal p_sngFINALDISTANCE As Single, _
                                  ByVal p_intFINALINFLATION As Integer, _
                                  ByVal p_dtePOSTCONDSTARTDATE As DateTime, _
                                  ByVal p_dtePOSTCONDENDDATE As DateTime, _
                                  ByVal p_intPOSTCONDENDTEMP As Integer, _
                                  ByVal p_strPASSYN As String, _
                                  ByVal p_intCertificationTypeID As Integer, _
                                  ByVal p_strCERTIFICATENUMBER As String, _
                                  ByVal p_dteSerialDate As DateTime, _
                                  ByVal p_sngPostCondTime As Single, _
                                  ByVal p_sngPreCondTime As Single, _
                                  ByVal p_sngDIAMETERTESTDRUM As Single, _
                                  ByVal p_sngPRECONDTEMP As Single, _
                                  ByVal p_sngINFLATIONPRESSUREREADJUSTED As Single, _
                                  ByVal p_sngCIRCUNFERENCEBEFORETEST As Single, _
                                  ByVal p_strRESULTPASSFAIL As String, _
                                  ByVal p_sngENDURANCEHOURS As Single, _
                                  ByVal p_strPOSSIBLEFAILURESFOUND As String, _
                                  ByVal p_sngCIRCUNFERENCEAFTERTEST As Single, _
                                  ByVal p_sngOUTERDIAMETERDIFERENCE As Single, _
                                  ByVal p_sngODDIFERENCETOLERANCE As Single, _
                                  ByVal p_strSERIENOM As String, _
                                  ByVal p_strFINALJUDGEMENT As String, _
                                  ByVal p_strAPPROVER As String, _
                                  ByVal p_strOperatorName As String, _
                                  ByVal p_intCertificateNumberID As Integer, _
                                  ByVal p_strMatlNum As String, _
                                  ByVal p_sngLowInfStartInflation As Single, _
                                  ByVal p_sngLowInfEndInflation As Single, _
                                  ByVal p_intLowInfEndTemp As Integer, _
                                  ByVal p_strOperation As String, _
                                  ByVal p_strGTSpecEndurance As String, _
                                  ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveEndurance
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveEndurance(p_intENDURANCEID, _
                                                       p_strProjectNumber, _
                                                       p_intTireNumber, _
                                                       p_strTESTSPEC, _
                                                       p_dteCOMPLETIONDATE, _
                                                       p_strDOTSERIALNUMBER, _
                                                       p_dtePRECONDSTARTDATE, _
                                                       p_sngPRECONDSTARTTEMP, _
                                                       p_sngRIMDIAMETER, _
                                                       p_sngRIMWIDTH, _
                                                       p_dtePRECONDENDDATE, _
                                                       p_intPRECONDENDTEMP, _
                                                       p_intINFLATIONPRESSURE, _
                                                       p_sngBEFOREDIAMETER, _
                                                       p_sngAFTERDIAMETER, _
                                                       p_intBEFOREINFLATION, _
                                                       p_intAFTERINFLATION, _
                                                       p_intWHEELPOSITION, _
                                                       p_intWHEELNUMBER, _
                                                       p_intFINALTEMP, _
                                                       p_sngFINALDISTANCE, _
                                                       p_intFINALINFLATION, _
                                                       p_dtePOSTCONDSTARTDATE, _
                                                       p_dtePOSTCONDENDDATE, _
                                                       p_intPOSTCONDENDTEMP, _
                                                       p_strPASSYN, _
                                                       p_intCertificationTypeID, _
                                                       p_strCERTIFICATENUMBER, _
                                                       p_dteSerialDate, _
                                                       p_sngPostCondTime, _
                                                       p_sngPreCondTime, _
                                                       p_sngDIAMETERTESTDRUM, _
                                                       p_sngPRECONDTEMP, _
                                                       p_sngINFLATIONPRESSUREREADJUSTED, _
                                                       p_sngCIRCUNFERENCEBEFORETEST, _
                                                       p_strRESULTPASSFAIL, _
                                                       p_sngENDURANCEHOURS, _
                                                       p_strPOSSIBLEFAILURESFOUND, _
                                                       p_sngCIRCUNFERENCEAFTERTEST, _
                                                       p_sngOUTERDIAMETERDIFERENCE, _
                                                       p_sngODDIFERENCETOLERANCE, _
                                                       p_strSERIENOM, _
                                                       p_strFINALJUDGEMENT, _
                                                       p_strAPPROVER, _
                                                       p_strOperatorName, _
                                                       p_intCertificateNumberID, _
                                                       p_strMatlNum, _
                                                       p_sngLowInfStartInflation, _
                                                       p_sngLowInfEndInflation, _
                                                       p_intLowInfEndTemp, _
                                                       p_strOperation, _
                                                       p_strGTSpecEndurance, _
                                                       p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveEnduranceDetail(ByVal p_intTESTSTEP As Integer, _
                                        ByVal p_intTIMEINMIN As Integer, _
                                        ByVal p_intSpeed As Integer, _
                                        ByVal p_sngTOTMILES As Single, _
                                        ByVal p_sngtLOAD As Single, _
                                        ByVal p_sngLOADPERCENT As Single, _
                                        ByVal p_intSETINFLATION As Integer, _
                                        ByVal p_intAMBTEMP As Integer, _
                                        ByVal p_intINFPRESSURE As Integer, _
                                        ByVal p_dteSTEPCOMPLETIONDATE As DateTime, _
                                        ByVal p_intENDURANCEID As Integer) As NameAid.SaveResult Implements IDepository.SaveEnduranceDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveEnduranceDetail(p_intTESTSTEP, _
                                                             p_intTIMEINMIN, _
                                                             p_intSpeed, _
                                                             p_sngTOTMILES, _
                                                             p_sngtLOAD, _
                                                             p_sngLOADPERCENT, _
                                                             p_intSETINFLATION, _
                                                             p_intAMBTEMP, _
                                                             p_intINFPRESSURE, _
                                                             p_dteSTEPCOMPLETIONDATE, _
                                                             p_intENDURANCEID)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveHighSpeed(ByRef p_intHighSpeedID As Integer, _
                                  ByVal p_strPROJECTNUMBER As String, _
                                  ByVal p_intTIRENUM As Integer, _
                                  ByVal p_strTESTSPEC As String, _
                                  ByVal p_dteCOMPETIONDATE As DateTime, _
                                  ByVal p_strDOTSERIALNUMBER As String, _
                                  ByVal p_strMFGWWYY As String, _
                                  ByVal p_dtePRECONDSTARTDATE As DateTime, _
                                  ByVal p_intPRECONDSARTTEMP As Integer, _
                                  ByVal p_sngRIMDIAMETER As Single, _
                                  ByVal p_sngRIMWIDTH As Single, _
                                  ByVal p_dtePRECONDENDDATE As DateTime, _
                                  ByVal p_intPRECONDENDTEMP As Integer, _
                                  ByVal p_intINFLATIONPRESSURE As Integer, _
                                  ByVal p_sngBEFOREDIAMETER As Single, _
                                  ByVal p_sngAFTERDIAMETER As Single, _
                                  ByVal p_intBEFOREINFLATION As Integer, _
                                  ByVal p_intAFTERINFLATION As Integer, _
                                  ByVal p_intWHEELPOSITION As Integer, _
                                  ByVal p_intWHEELNUMBER As Integer, _
                                  ByVal p_intFINALTEMP As Integer, _
                                  ByVal p_sngFINALDISTANCE As Single, _
                                  ByVal p_intFINALINFLATION As Integer, _
                                  ByVal p_dtePOSTCONDSTARTDATE As DateTime, _
                                  ByVal p_dtePOSTCONDENDDATE As DateTime, _
                                  ByVal p_intPOSTCONDENDTEMP As Integer, _
                                  ByVal p_sngPRECONDTIME As Single, _
                                  ByVal p_sngPOSTCONDTIME As Single, _
                                  ByVal p_strPASSYN As String, _
                                  ByVal p_dteSERIALDATE As DateTime, _
                                  ByVal p_intCERTIFICATIONTYPEID As Integer, _
                                  ByVal p_strCERTIFICATENUMBER As String, _
                                  ByVal p_sngDIAMETERTESTDRUM As Single, _
                                  ByVal p_sngPRECONDTEMP As Single, _
                                  ByVal p_sngINFLATIONPRESSUREREADJUSTED As Single, _
                                  ByVal p_sngCIRCUNFERENCEBEFORETEST As Single, _
                                  ByVal p_sngWHEELSPEEDRPM As Single, _
                                  ByVal p_sngWHEELSPEEDKMH As Single, _
                                  ByVal p_sngCIRCUNFERENCEAFTERTEST As Single, _
                                  ByVal p_sngODDIFERENCE As Single, _
                                  ByVal p_sngODDIFERENCETOLERANCE As Single, _
                                  ByVal p_strSERIENOM As String, _
                                  ByVal p_strFINALJUDGEMENT As String, _
                                  ByVal p_strAPPROVER As String, _
                                  ByVal p_sngPASSATKMH As Single, _
                                  ByVal p_strSPEEDTTESTPASSFAIL As String, _
                                  ByVal p_sngSPEEDTOTALTIME As Single, _
                                  ByVal p_sngMAXSPEED As Single, _
                                  ByVal p_sngMAXLOAD As Single, _
                                  ByVal p_strOperatorName As String, _
                                  ByVal p_intCertificateNumberID As Integer, _
                                  ByVal p_strMatlNum As String, _
                                  ByVal p_strOperation As String, _
                                  ByVal p_strGTSpecHighSpeed As String) As NameAid.SaveResult Implements IDepository.SaveHighSpeed
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveHighSpeed(p_intHighSpeedID, _
                                                       p_strPROJECTNUMBER, _
                                                       p_intTIRENUM, _
                                                       p_strTESTSPEC, _
                                                       p_dteCOMPETIONDATE, _
                                                       p_strDOTSERIALNUMBER, _
                                                       p_strMFGWWYY, _
                                                       p_dtePRECONDSTARTDATE, _
                                                       p_intPRECONDSARTTEMP, _
                                                       p_sngRIMDIAMETER, _
                                                       p_sngRIMWIDTH, _
                                                       p_dtePRECONDENDDATE, _
                                                       p_intPRECONDENDTEMP, _
                                                       p_intINFLATIONPRESSURE, _
                                                       p_sngBEFOREDIAMETER, _
                                                       p_sngAFTERDIAMETER, _
                                                       p_intBEFOREINFLATION, _
                                                       p_intAFTERINFLATION, _
                                                       p_intWHEELPOSITION, _
                                                       p_intWHEELNUMBER, _
                                                       p_intFINALTEMP, _
                                                       p_sngFINALDISTANCE, _
                                                       p_intFINALINFLATION, _
                                                       p_dtePOSTCONDSTARTDATE, _
                                                       p_dtePOSTCONDENDDATE, _
                                                       p_intPOSTCONDENDTEMP, _
                                                       p_sngPRECONDTIME, _
                                                       p_sngPOSTCONDTIME, _
                                                       p_strPASSYN, _
                                                       p_dteSERIALDATE, _
                                                       p_intCERTIFICATIONTYPEID, _
                                                       p_strCERTIFICATENUMBER, _
                                                       p_sngDIAMETERTESTDRUM, _
                                                       p_sngPRECONDTEMP, _
                                                       p_sngINFLATIONPRESSUREREADJUSTED, _
                                                       p_sngCIRCUNFERENCEBEFORETEST, _
                                                       p_sngWHEELSPEEDRPM, _
                                                       p_sngWHEELSPEEDKMH, _
                                                       p_sngCIRCUNFERENCEAFTERTEST, _
                                                       p_sngODDIFERENCE, _
                                                       p_sngODDIFERENCETOLERANCE, _
                                                       p_strSERIENOM, _
                                                       p_strFINALJUDGEMENT, _
                                                       p_strAPPROVER, _
                                                       p_sngPASSATKMH, _
                                                       p_strSPEEDTTESTPASSFAIL, _
                                                       p_sngSPEEDTOTALTIME, _
                                                       p_sngMAXSPEED, _
                                                       p_sngMAXLOAD, _
                                                       p_strOperatorName, _
                                                       p_intCertificateNumberID, _
                                                       p_strMatlNum, _
                                                       p_strOperation, _
                                                       p_strGTSpecHighSpeed)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveHighSpeedDetail(ByVal p_intHighSpeedID As Integer, _
                                        ByVal p_strOperatorId As String, _
                                        ByVal p_intTESTSTEP As Integer, _
                                        ByVal p_intTimeMin As Integer, _
                                        ByVal p_sngSpeed As Single, _
                                        ByVal p_sngTotMiles As Single, _
                                        ByVal p_sngLoad As Single, _
                                        ByVal p_intLoadPercent As Single, _
                                        ByVal p_intSetInflation As Integer, _
                                        ByVal p_intAmbTemp As Integer, _
                                        ByVal p_intInfPressure As Integer, _
                                        ByVal p_dteStepCompletionDate As DateTime) As NameAid.SaveResult Implements IDepository.SaveHighSpeedDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveHighSpeedDetail(p_intHighSpeedID, _
                                                             p_strOperatorId, _
                                                             p_intTESTSTEP, _
                                                             p_intTimeMin, _
                                                             p_sngSpeed, _
                                                             p_sngTotMiles, _
                                                             p_sngLoad, _
                                                             p_intLoadPercent, _
                                                             p_intSetInflation, _
                                                             p_intAmbTemp, _
                                                             p_intInfPressure, _
                                                             p_dteStepCompletionDate)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveHighSpeed_SpeedTestDetail(ByVal p_intHighSpeedID As Integer, _
                                                  ByVal p_intIteration As Integer, _
                                                  ByVal p_dteTime As DateTime, _
                                                  ByVal p_sngSpeed As Single, _
                                                  ByVal p_strUserName As String) As NameAid.SaveResult Implements IDepository.SaveHighSpeed_SpeedTestDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveHighSpeed_SpeedTestDetail(p_intHighSpeedID, _
                                                                       p_intIteration, _
                                                                       p_dteTime, _
                                                                       p_sngSpeed, _
                                                                       p_strUserName)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveSound(ByVal p_strUserId As String, _
                              ByRef p_intSoundID As Integer, _
                              ByVal p_strPROJECTNUMBER As String, _
                              ByVal p_intTIRENUM As Integer, _
                              ByVal p_strTESTSPEC As String, _
                              ByVal p_strTESTREPORTNUMBER As String, _
                              ByVal p_strMANUFACTUREANDBRAND As String, _
                              ByVal p_strTIRECLASS As String, _
                              ByVal p_strCATEGORYOFUSE As String, _
                              ByVal p_dteDATEOFTEST As DateTime, _
                              ByVal p_strTESTVEHICULE As String, _
                              ByVal p_strTESTVEHICULEWHEELBASE As String, _
                              ByVal p_strLOCATIONOFTESTTRACK As String, _
                              ByVal p_dteDATETRACKCERTIFTOISO As DateTime, _
                              ByVal p_strTIRESIZEDESIGNATION As String, _
                              ByVal p_strTIRESERVICEDESCRIPTION As String, _
                              ByVal p_strREFERENCEINFLATIONPRESSURE As String, _
                              ByVal p_strTESTMASS_FRONTL As String, _
                              ByVal p_strTESTMASS_FRONTR As String, _
                              ByVal p_strTESTMASS_REARL As String, _
                              ByVal p_strTESTMASS_REARR As String, _
                              ByVal p_strTIRELOADINDEX_FRONTL As String, _
                              ByVal p_strTIRELOADINDEX_FRONTR As String, _
                              ByVal p_strTIRELOADINDEX_REARL As String, _
                              ByVal p_strTIRELOADINDEX_REARR As String, _
                              ByVal p_strINFLATIONPRESSURECO_FRONTL As String, _
                              ByVal p_strINFLATIONPRESSURECO_FRONTR As String, _
                              ByVal p_strINFLATIONPRESSURECO_REARL As String, _
                              ByVal p_strINFLATIONPRESSURECO_REARR As String, _
                              ByVal p_strTESTRIMWIDTHCODE As String, _
                              ByVal p_strTEMPMEASURESENSORTYPE As String, _
                              ByVal p_intCERTIFICATIONTYPEID As Integer, _
                              ByVal p_strCERTIFICATENUMBER As String, _
                              ByVal p_intSKUID As Integer, _
                              ByVal p_intCertificateNUmberID As Integer, _
                              ByVal p_strOperation As String, _
                              ByVal p_strGTSpecSound As String, _
                              ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveSound
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveSound(p_strUserId, _
                                                   p_intSoundID, _
                                                   p_strPROJECTNUMBER, _
                                                   p_intTIRENUM, _
                                                   p_strTESTSPEC, _
                                                   p_strTESTREPORTNUMBER, _
                                                   p_strMANUFACTUREANDBRAND, _
                                                   p_strTIRECLASS, _
                                                   p_strCATEGORYOFUSE, _
                                                   p_dteDATEOFTEST, _
                                                   p_strTESTVEHICULE, _
                                                   p_strTESTVEHICULEWHEELBASE, _
                                                   p_strLOCATIONOFTESTTRACK, _
                                                   p_dteDATETRACKCERTIFTOISO, _
                                                   p_strTIRESIZEDESIGNATION, _
                                                   p_strTIRESERVICEDESCRIPTION, _
                                                   p_strREFERENCEINFLATIONPRESSURE, _
                                                   p_strTESTMASS_FRONTL, _
                                                   p_strTESTMASS_FRONTR, _
                                                   p_strTESTMASS_REARL, _
                                                   p_strTESTMASS_REARR, _
                                                   p_strTIRELOADINDEX_FRONTL, _
                                                   p_strTIRELOADINDEX_FRONTR, _
                                                   p_strTIRELOADINDEX_REARL, _
                                                   p_strTIRELOADINDEX_REARR, _
                                                   p_strINFLATIONPRESSURECO_FRONTL, _
                                                   p_strINFLATIONPRESSURECO_FRONTR, _
                                                   p_strINFLATIONPRESSURECO_REARL, _
                                                   p_strINFLATIONPRESSURECO_REARR, _
                                                   p_strTESTRIMWIDTHCODE, _
                                                   p_strTEMPMEASURESENSORTYPE, _
                                                   p_intCERTIFICATIONTYPEID, _
                                                   p_strCERTIFICATENUMBER, _
                                                   p_intSKUID, _
                                                   p_intCertificateNUmberID, _
                                                   p_strOperation, _
                                                   p_strGTSpecSound, _
                                                   p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveSoundDetail(ByVal p_strUserId As String, _
                                    ByVal p_intITERATION As Integer, _
                                    ByVal p_strTESTSPEED As String, _
                                    ByVal p_strDIRECTIONOFRUN As String, _
                                    ByVal p_strSOUNDLEVELLEFT As String, _
                                    ByVal p_strSOUNDLEVELRIGHT As String, _
                                    ByVal p_strAIRTEMP As String, _
                                    ByVal p_strTRACKTEMP As String, _
                                    ByVal p_strSOUNDLEVELLEFT_TEMPCOR As String, _
                                    ByVal p_strSOUNDLEVELRIGHT_TEMPCOR As String, _
                                    ByVal p_intSoundID As Integer) As NameAid.SaveResult Implements IDepository.SaveSoundDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveSoundDetail(p_strUserId, _
                                                         p_intITERATION, _
                                                         p_strTESTSPEED, _
                                                         p_strDIRECTIONOFRUN, _
                                                         p_strSOUNDLEVELLEFT, _
                                                         p_strSOUNDLEVELRIGHT, _
                                                         p_strAIRTEMP, _
                                                         p_strTRACKTEMP, _
                                                         p_strSOUNDLEVELLEFT_TEMPCOR, _
                                                         p_strSOUNDLEVELRIGHT_TEMPCOR, _
                                                         p_intSoundID)
        End Using

        Return enumSaveResult
    End Function

    'Added Operation as per PRJ3617 SAP Interface to International Certification System (OPQ.I.8265)Remediation
    Public Function SaveWetGrip(ByVal p_strUserId As String, _
                                ByRef p_intWetGripID As Integer, _
                                ByVal p_strPROJECTNUMBER As String, _
                                ByVal p_intTIRENUM As Integer, _
                                ByVal p_strTESTSPEC As String, _
                                ByVal p_dteDATEOFTEST As DateTime, _
                                ByVal p_strTESTVEHICLE As String, _
                                ByVal p_strLOCATIONOFTESTTRACK As String, _
                                ByVal p_strTESTTRACKCHARACTERISTICS As String, _
                                ByVal p_strISSUEBY As String, _
                                ByVal p_strMETHODOFCERTIFICATION As String, _
                                ByVal p_strTESTTIREDETAILS As String, _
                                ByVal p_strTIRESIZEANDSERVICEDESC As String, _
                                ByVal p_strTIREBRANDANDTRADEDESC As String, _
                                ByVal p_strREFERENCEINFLATIONPRESSURE As String, _
                                ByVal p_strTESTRIMWITHCODE As String, _
                                ByVal p_strTEMPMEASURESENSORTYPE As String, _
                                ByVal p_strIDENTIFICATIONSRTT As String, _
                                ByVal p_strTESTTIRELOAD_SRTT As String, _
                                ByVal p_strTESTTIRELOAD_CANDIDATE As String, _
                                ByVal p_strTESTTIRELOAD_CONTROL As String, _
                                ByVal p_strWATERDEPTH_SRTT As String, _
                                ByVal p_strWATERDEPTH_CANDIDATE As String, _
                                ByVal p_strWATERDEPTH_CONTROL As String, _
                                ByVal p_strWETTEDTRACKTEMPAVG As String, _
                                ByVal p_intCERTIFICATIONTYPEID As Integer, _
                                ByVal p_strCERTIFICATENUMBER As String, _
                                ByVal p_intSKUID As Integer, _
                                ByVal p_intCertificateNUmberID As Integer, _
                                ByVal p_strOperation As String, _
                                ByVal p_strGTSpecWetGrip As String, _
                                ByVal p_strMFGWWYY As String) As NameAid.SaveResult Implements IDepository.SaveWetGrip
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveWetGrip(p_strUserId, _
                                                     p_intWetGripID, _
                                                     p_strPROJECTNUMBER, _
                                                     p_intTIRENUM, _
                                                     p_strTESTSPEC, _
                                                     p_dteDATEOFTEST, _
                                                     p_strTESTVEHICLE, _
                                                     p_strLOCATIONOFTESTTRACK, _
                                                     p_strTESTTRACKCHARACTERISTICS, _
                                                     p_strISSUEBY, _
                                                     p_strMETHODOFCERTIFICATION, _
                                                     p_strTESTTIREDETAILS, _
                                                     p_strTIRESIZEANDSERVICEDESC, _
                                                     p_strTIREBRANDANDTRADEDESC, _
                                                     p_strREFERENCEINFLATIONPRESSURE, _
                                                     p_strTESTRIMWITHCODE, _
                                                     p_strTEMPMEASURESENSORTYPE, _
                                                     p_strIDENTIFICATIONSRTT, _
                                                     p_strTESTTIRELOAD_SRTT, _
                                                     p_strTESTTIRELOAD_CANDIDATE, _
                                                     p_strTESTTIRELOAD_CONTROL, _
                                                     p_strWATERDEPTH_SRTT, _
                                                     p_strWATERDEPTH_CANDIDATE, _
                                                     p_strWATERDEPTH_CONTROL, _
                                                     p_strWETTEDTRACKTEMPAVG, _
                                                     p_intCERTIFICATIONTYPEID, _
                                                     p_strCERTIFICATENUMBER, _
                                                     p_intSKUID, _
                                                     p_intCertificateNUmberID, _
                                                     p_strOperation, _
                                                     p_strGTSpecWetGrip, _
                                                     p_strMFGWWYY)
        End Using

        Return enumSaveResult
    End Function

    Public Function SaveWetGripDetail(ByVal p_strUserId As String, _
                                      ByVal p_intITERATION As Integer, _
                                      ByVal p_strTESTSPEED As String, _
                                      ByVal p_strDIRECTIONOFRUN As String, _
                                      ByVal p_strSRTT As String, _
                                      ByVal p_strCANDIDATETIRE As String, _
                                      ByVal p_strPEAKBREAKFORCECOEFICIENT As String, _
                                      ByVal p_strMEANFULLYDEVDECELERATION As String, _
                                      ByVal p_strWETGRIPINDEX As String, _
                                      ByVal p_strCOMMENTS As String, _
                                      ByVal p_intWetGripID As Integer) As NameAid.SaveResult Implements IDepository.SaveWetGripDetail
        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.SaveWetGripDetail(p_strUserId, _
                                                           p_intITERATION, _
                                                           p_strTESTSPEED, _
                                                           p_strDIRECTIONOFRUN, _
                                                           p_strSRTT, _
                                                           p_strCANDIDATETIRE, _
                                                           p_strPEAKBREAKFORCECOEFICIENT, _
                                                           p_strMEANFULLYDEVDECELERATION, _
                                                           p_strWETGRIPINDEX, _
                                                           p_strCOMMENTS, _
                                                           p_intWetGripID)
        End Using

        Return enumSaveResult
    End Function

    Public Function AddCustomer(ByVal p_intSKUId As Integer, _
                                ByVal p_strCustomer As String, _
                                ByVal p_strImporter As String, _
                                ByVal p_strImporterRepresentative As String, _
                                ByVal p_strImporterAddress As String, _
                                ByVal p_strCountryLocation As String) As NameAid.SaveResult Implements IDepository.AddCustomer

        Dim enumSaveResult As NameAid.SaveResult = NameAid.SaveResult.SaveError

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            enumSaveResult = ctdCertDalc.AddCustomer(p_intSKUId, _
                                                     p_strCustomer, _
                                                     p_strImporter, _
                                                     p_strImporterRepresentative, _
                                                     p_strImporterAddress, _
                                                     p_strCountryLocation)
        End Using

        Return enumSaveResult

    End Function

    Public Function GetCertificateIDByNumber(ByVal p_strCertificateNumber As String, ByVal p_intCertificateTypeId As Integer, ByVal p_strExtensionNo As String) As Integer Implements IDepository.GetCertificateID

        Dim intCertificateID As Integer
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            intCertificateID = ctdCertDalc.GetCertificateID(p_strCertificateNumber, p_intCertificateTypeId, p_strExtensionNo)
        End Using

        Return intCertificateID

    End Function

    Public Function GetCertificationTypeID(ByVal p_strCertificationTypeName As String) As Integer Implements IDepository.GetCertificationTypeID
        Dim intCertificationTypeID As Integer
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            intCertificationTypeID = ctdCertDalc.GetCertificationTypeID(p_strCertificationTypeName)
        End Using

        Return intCertificationTypeID

    End Function

    ' added for generic certifcation types 6/9/2016 - jeseitz
    Public Function GetCertTemplate(ByVal p_strCertificationTypeName As String) As String Implements IDepository.GetCertTemplate
        Dim strCertTemplate As String
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strCertTemplate = ctdCertDalc.GetCertTemplate(p_strCertificationTypeName)
        End Using

        Return strCertTemplate

    End Function

    Function GetCertificationNameByID(ByVal p_intCertificationTypeID As Integer) As String Implements IDepository.GetCertificationNameByID
        Dim strCertificationName As String
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strCertificationName = ctdCertDalc.GetCertificationNameByID(p_intCertificationTypeID)
        End Using

        Return strCertificationName

    End Function

    ' Added as per project 2706 technical specification
    Public Function GetCertifiedMaterialCount(ByVal p_intCertificationTypeId As Integer, _
                                              ByVal p_strCertificateNumber As String, _
                                              ByVal p_strCertificateExtension As String) As Integer Implements IDepository.GetCertifiedMaterialCount

        Dim intCertificateID As Integer = 0
        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            intCertificateID = ctdCertDalc.GetCertifiedMaterialCount(p_intCertificationTypeId, _
                                                                     p_strCertificateNumber, _
                                                                     p_strCertificateExtension)
        End Using

        Return intCertificateID

    End Function

    Public Function RenameCertificate(ByVal p_intCertificationTypeId As Integer, _
                                      ByVal p_strOldCertificateNumber As String, _
                                      ByVal p_strOldCertificateExtension As String, _
                                      ByVal p_strNewCertificateNumber As String, _
                                      ByVal p_strNewCertificateExtension As String, _
                                      ByVal p_strUserName As String) As Boolean Implements IDepository.RenameCertificate

        Dim blnDone As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.RenameCertificate(p_intCertificationTypeId, _
                                                    p_strOldCertificateNumber, _
                                                    p_strOldCertificateExtension, _
                                                    p_strNewCertificateNumber, _
                                                    p_strNewCertificateExtension, _
                                                    p_strUserName)
        End Using

        Return blnDone

    End Function

    Public Function DeleteCertificate(ByVal p_intCertificationTypeId As Integer, _
                                      ByVal p_strCertificateNumber As String, _
                                      ByVal p_strCertificateExtension As String, _
                                      ByVal p_strUserName As String) As Boolean Implements IDepository.DeleteCertificate

        Dim blnDone As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.DeleteCertificate(p_intCertificationTypeId, _
                                                    p_strCertificateNumber, _
                                                    p_strCertificateExtension, _
                                                    p_strUserName)
        End Using

        Return blnDone

    End Function

    Public Function GetCertificateMaterials(ByVal p_intCertificationTypeId As Integer, _
                                            ByVal p_strCertificateNumber As String, _
                                            ByVal p_strCertificateExtension As String) As DataTable Implements IDepository.GetCertificateMaterials
        Dim dtCertificateMaterials As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtCertificateMaterials = ctdCertDalc.GetCertificateMaterials(p_intCertificationTypeId, _
                                                                         p_strCertificateNumber, _
                                                                         p_strCertificateExtension)
        End Using

        Return dtCertificateMaterials
    End Function

    Public Function DetachCertificate(ByVal p_intSkuId As Integer, _
                                      ByVal p_intCertificateId As Integer) As Boolean Implements IDepository.DetachCertificate

        Dim blnDone As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.DetachCertificate(p_intSkuId, _
                                                    p_intCertificateId)
        End Using

        Return blnDone

    End Function

    Public Function MoveCertificate(ByVal p_intCertificationTypeId As Integer, _
                                    ByVal p_strNewCertificateNumber As String, _
                                    ByVal p_strNewCertificateExtension As String, _
                                    ByVal p_intSkuId As Integer, _
                                    ByVal p_intCertificateId As Integer, _
                                    ByVal p_strUserName As String) As Boolean Implements IDepository.MoveCertificate

        Dim blnDone As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDone = ctdCertDalc.MoveCertificate(p_intCertificationTypeId, _
                                                  p_strNewCertificateNumber, _
                                                  p_strNewCertificateExtension, _
                                                  p_intSkuId, _
                                                  p_intCertificateId, _
                                                  p_strUserName)
        End Using

        Return blnDone

    End Function

    Public Function GetDuplicateCertificates(ByVal p_strMaterialNumber As String, _
                                             ByVal p_strSpeedRating As String) As DataTable Implements IDepository.GetDuplicateCertificates

        Dim dtResults As New DataTable

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtResults = ctdCertDalc.GetDuplicateCertificates(p_strMaterialNumber, _
                                                             p_strSpeedRating)
        End Using

        Return dtResults

    End Function

    Public Function DeleteDuplicateCertificates(ByVal p_intSkuId As Integer) As Boolean Implements IDepository.DeleteDuplicateCertificates

        Dim blnDeleted As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDeleted = ctdCertDalc.DeleteDuplicateCertificates(p_intSkuId)
        End Using

        Return blnDeleted

    End Function

    ''' <summary>
    ''' Check whether the family Id exists or not and get the Family Desc
    ''' </summary>
    ''' <param name="p_intFamilyId">FamilyID</param>
    ''' <param name="p_strFamilyDesc">Family Desc</param>
    ''' <returns>boolean value</returns>
    ''' <remarks></remarks>
    Public Function CheckIsFamilyIdExist(ByVal p_intcertificateid As Integer, _
                                        ByVal p_intFamilyId As String, _
                                        ByRef p_strFamilyDesc As String) As Boolean Implements IDepository.CheckIsFamilyIdExist
        Dim blnExists As Boolean

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnExists = ctdCertDalc.CheckIsFamilyIdExist(p_intcertificateid, p_intFamilyId, p_strFamilyDesc)
        End Using

        Return blnExists

    End Function
    ''' <summary>
    ''' Gets type's of Tyres.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTireType() As DataTable Implements IDepository.GetTireType
        Dim dtTireType As DataTable = Nothing

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtTireType = ctdCertDalc.GetTireType()
        End Using

        Return dtTireType
    End Function


    ''' <summary>
    ''' Inserts/updates the record in Imark family table
    ''' </summary>
    ''' <param name="p_intFamilyID">FamilyID </param>
    ''' <param name="p_strFamilyCode">FamilyCode</param>
    ''' <param name="p_strFamilyDesc">FamilyDesc</param>
    ''' <param name="p_strApplicationCat">ApplicationCat</param>
    ''' <param name="p_strConstructionType">ConstructionType</param>
    ''' <param name="p_strStructureType">StructureType</param>
    ''' <param name="p_strMountingType">MountingType</param>
    ''' <param name="p_strAspectRatioCat">AspectRatioCat</param>
    ''' <param name="p_strSpeedRatingCat">SpeedRatingCat</param>
    ''' <param name="p_strLoadIndexCat">LoadIndexCat</param>    
    ''' <param name="p_strUserName">Username</param>  
    ''' <returns>boolean value</returns>
    ''' <remarks></remarks>
    Public Function SaveFamily(ByVal p_intCertificateid As Integer, _
                                ByVal p_intFamilyID As Integer, _
                                ByVal p_strFamilyCode As String, _
                                ByVal p_strFamilyDesc As String, _
                                ByVal p_strApplicationCat As String, _
                                ByVal p_strConstructionType As String, _
                                ByVal p_strStructureType As String, _
                                ByVal p_strMountingType As String, _
                                ByVal p_strAspectRatioCat As String, _
                                ByVal p_strSpeedRatingCat As String, _
                                ByVal p_strLoadIndexCat As String, _
                                ByVal p_strUserName As String) As System.Boolean Implements IDepository.SaveFamily

        Dim blnSaved As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnSaved = ctdCertDalc.SaveFamily(p_intCertificateid, _
                                             p_intFamilyID, _
                                             p_strFamilyCode, _
                                             p_strFamilyDesc, _
                                             p_strApplicationCat, _
                                             p_strConstructionType, _
                                             p_strStructureType, _
                                             p_strMountingType, _
                                             p_strAspectRatioCat, _
                                             p_strSpeedRatingCat, _
                                             p_strLoadIndexCat, _
                                             p_strUserName)

        End Using

        Return blnSaved

    End Function

    ''' <summary>
    ''' Deletes the data from Imark family table
    ''' </summary>
    ''' <param name="p_intFamilyId">FamilyID</param>
    ''' <returns>boolean value</returns>
    ''' <remarks></remarks>
    Public Function Deletefamily(ByVal p_intCertificateid As Integer, _
                                 ByVal p_intFamilyId As Integer) As Boolean Implements IDepository.Deletefamily

        Dim blnDeleted As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnDeleted = ctdCertDalc.Deletefamily(p_intCertificateid, p_intFamilyId)

        End Using

        Return blnDeleted
    End Function

    ''' <summary>
    ''' Get All Families from database
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetFamilies(ByVal pn_certificateid As Integer) As DataTable Implements IDepository.GetFamilies
        Dim dtbResults As DataTable = Nothing

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetFamilies(pn_certificateid)
        End Using
        Return dtbResults
    End Function

    ''' <summary>
    ''' Copy Certification
    ''' </summary>
    ''' <param name="p_strMatlNum">Material Number</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function CopyCertification(ByVal p_strMatlNum As String) As Boolean Implements IDepository.CopyCertification
        Dim blnCopied As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnCopied = ctdCertDalc.CopyCertification(p_strMatlNum)
        End Using
        Return blnCopied
    End Function

    ''' <summary>
    ''' Attach Certification
    ''' </summary>
    ''' <param name="p_skuid">Sku Id</param>
    ''' <param name="p_strCertNum">Certificate Number</param>
    ''' <param name="p_strExtensionEn">Extension Number</param>
    ''' <param name="p_certificationtypeid">Certificate Type Id</param>
    ''' <returns>Error Message.</returns>
    ''' <remarks></remarks>
    Function AttachCertification(ByVal p_skuid As Integer, _
                                 ByVal p_strCertNum As String, _
                                 ByVal p_strExtensionEn As String, _
                                 ByVal p_certificationtypeid As Integer) As String Implements IDepository.AttachCertification
        Dim strResult As String = String.Empty

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            strResult = ctdCertDalc.AttachCertification(p_skuid, p_strCertNum, p_strExtensionEn, p_certificationtypeid)
        End Using
        Return strResult
    End Function

    ''' <summary>
    ''' Update Speedrating of a Material
    ''' </summary>
    ''' <param name="p_intSkuID"></param>
    ''' <param name="p_strSpeedrating"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function EditMaterial(ByVal p_intSkuID As Integer, _
                                 ByVal p_strSpeedrating As String) As Boolean Implements IDepository.EditMaterial
        Dim blnSaved As Boolean = False

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            blnSaved = ctdCertDalc.EditMaterial(p_intSkuID, p_strSpeedrating)
        End Using
        Return blnSaved
    End Function

    ''' <summary>
    ''' Get Material Details
    ''' </summary>
    ''' <param name="p_strMaterialNumber"></param>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetMaterial(ByVal p_strMaterialNumber As String) As DataTable Implements IDepository.GetMaterial
        Dim dtbResults As DataTable = Nothing

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            dtbResults = ctdCertDalc.GetMaterial(p_strMaterialNumber)
        End Using
        Return dtbResults
    End Function

    ''' <summary>
    ''' Refreshes Product data.
    ''' </summary>
    ''' <param name="p_strMaterialNumber">Material Number</param>
    ''' <param name="p_strErrorDesc">Error Description</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RefreshProduct(ByVal p_strMaterialNumber As String, ByRef p_strErrorDesc As String) As Integer Implements IDepository.RefreshProduct
        Dim errNumber As Integer = 0

        Using ctdCertDalc As CertificationDalc = New CertificationDalc
            errNumber = ctdCertDalc.RefreshProduct(p_strMaterialNumber, p_strErrorDesc)
        End Using

        Return errNumber
    End Function

End Class
