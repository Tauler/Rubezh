#include "StdAfx.h"
#include "WrapHolidays.h"

#include <iostream>
#include <fstream>
using namespace std;

#define QUERY_COUNT	(3)

int CALL_METHOD WRAP_Insert_Holiday(int loginID, NET_RECORDSET_HOLIDAY* param)
{
	if (NULL == loginID)
	{
		return 0;
	}
	
	NET_CTRL_RECORDSET_INSERT_PARAM stuInert = {sizeof(stuInert)};
	stuInert.stuCtrlRecordSetInfo.dwSize = sizeof(NET_CTRL_RECORDSET_INSERT_IN);
    stuInert.stuCtrlRecordSetInfo.emType = NET_RECORD_ACCESSCTLHOLIDAY;
	stuInert.stuCtrlRecordSetInfo.pBuf = (void*)param;
	stuInert.stuCtrlRecordSetInfo.nBufLen = sizeof(NET_RECORDSET_HOLIDAY);
	
	stuInert.stuCtrlRecordSetResult.dwSize = sizeof(NET_CTRL_RECORDSET_INSERT_OUT);
    BOOL bResult = CLIENT_ControlDevice(loginID, DH_CTRL_RECORDSET_INSERT, &stuInert, SDK_API_WAITTIME);
	int nRecrdNo = stuInert.stuCtrlRecordSetResult.nRecNo;
	if (bResult)
	{
		return nRecrdNo;
	}
	return 0;
}

BOOL CALL_METHOD WRAP_Update_Holiday(int loginID, NET_RECORDSET_HOLIDAY* param)
{
	if (NULL == loginID)
	{
		return FALSE;
	}

	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
    stuInert.emType = NET_RECORD_ACCESSCTLHOLIDAY;
	stuInert.pBuf = (void*)param;
	stuInert.nBufLen = sizeof(NET_RECORDSET_HOLIDAY);
	
    BOOL bResult = CLIENT_ControlDevice(loginID, DH_CTRL_RECORDSET_UPDATE, &stuInert, SDK_API_WAITTIME);
    return bResult;
}

BOOL CALL_METHOD WRAP_Remove_Holiday(int loginID, int recordNo)
{
	if (NULL == loginID)
	{
		return FALSE;
	}
	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
	stuInert.emType = NET_RECORD_ACCESSCTLHOLIDAY;
	stuInert.pBuf = (void*)&recordNo;
	stuInert.nBufLen = sizeof(recordNo);
    BOOL bResult = CLIENT_ControlDevice(loginID, DH_CTRL_RECORDSET_REMOVE, &stuInert, SDK_API_WAITTIME);
	return bResult;
}

BOOL CALL_METHOD WRAP_RemoveAll_Holidays(int loginID)
{
	if (NULL == loginID)
	{
		return FALSE;
	}
	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
	stuInert.emType = NET_RECORD_ACCESSCTLHOLIDAY;
    BOOL bResult = CLIENT_ControlDevice(loginID, DH_CTRL_RECORDSET_CLEAR, &stuInert, SDK_API_WAITTIME);
	return bResult;
}

BOOL CALL_METHOD WRAP_Get_Holiday_Info(int loginID, int recordNo, NET_RECORDSET_HOLIDAY* result)
{
	if (NULL == loginID)
	{
		return FALSE;
	}

	NET_CTRL_RECORDSET_PARAM stuParam = {sizeof(stuParam)};
	stuParam.emType = NET_RECORD_ACCESSCTLHOLIDAY;
		
	NET_RECORDSET_HOLIDAY stuHoliday = {sizeof(stuHoliday)};
	stuHoliday.nRecNo = recordNo;
	stuParam.pBuf = &stuHoliday;
		
	int nRet = 0;
	BOOL bRet = CLIENT_QueryDevState(loginID, DH_DEVSTATE_DEV_RECORDSET, (char*)&stuParam, sizeof(stuParam), &nRet, SDK_API_WAITTIME);
	memcpy(result, &stuHoliday, sizeof(stuHoliday));
	return bRet;
}

void WRAP_testRecordSetFind_Holiday(LLONG loginID, LLONG& lFinderId)
{
	NET_IN_FIND_RECORD_PARAM stuIn = {sizeof(stuIn)};
	NET_OUT_FIND_RECORD_PARAM stuOut = {sizeof(stuOut)};
	
	stuIn.emType = NET_RECORD_ACCESSCTLHOLIDAY;

	stuIn.pQueryCondition = NULL;
	
	if (CLIENT_FindRecord(loginID, &stuIn, &stuOut, SDK_API_WAITTIME))
	{
		lFinderId = stuOut.lFindeHandle;
		printf("WRAP_testRecordSetFind_Holiday ok!\n");
	}
	else
	{
		printf("WRAP_testRecordSetFind_Holiday failed:0x%08x!\n", CLIENT_GetLastError());
	}		
}

void WRAP_testRecordSetFindNext_Holiday(LLONG lFinderId)
{
	NET_IN_FIND_NEXT_RECORD_PARAM stuIn = {sizeof(stuIn)};
	stuIn.lFindeHandle = lFinderId;
	stuIn.nFileCount = QUERY_COUNT;
	
	NET_OUT_FIND_NEXT_RECORD_PARAM stuOut = {sizeof(stuOut)};
	stuOut.nMaxRecordNum = stuIn.nFileCount;
	
	NET_RECORDSET_HOLIDAY stuHoliday[QUERY_COUNT] = {0};
	for (int i = 0; i < sizeof(stuHoliday)/sizeof(stuHoliday[0]); i++)
	{
		stuHoliday[i].dwSize = sizeof(NET_RECORDSET_HOLIDAY);
	}
	stuOut.pRecordList = (void*)&stuHoliday[0];
	
	if (CLIENT_FindNextRecord(&stuIn, &stuOut, SDK_API_WAITTIME) >= 0)
	{
		printf("WRAP_testRecordSetFindNext_Holiday ok!\n");
		
		for (int j = 0; j < stuOut.nRetRecordNum; j++)
		{
			printf("Enable:%s, RecNo:%d\n",
				stuHoliday[j].bEnable ? "Yes" : "No",
				stuHoliday[j].nRecNo);
		}
	}
	else
	{
		printf("WRAP_testRecordSetFindNext_Holiday failed:0x%08x!\n", CLIENT_GetLastError());
	}		
}

int GetHolidaysCountRecordSetFind(LLONG& lFinderId)
{
	NET_IN_QUEYT_RECORD_COUNT_PARAM stuIn = {sizeof(stuIn)};
	NET_OUT_QUEYT_RECORD_COUNT_PARAM stuOut = {sizeof(stuOut)};
	stuIn.lFindeHandle = lFinderId;
	if (CLIENT_QueryRecordCount(&stuIn, &stuOut, SDK_API_WAITTIME))
	{
		return stuOut.nRecordCount;
	}
	else
	{
		return 0;
	}
}

int CALL_METHOD WRAP_Get_Holidays_Count(int loginID)
{
	if (NULL == loginID)
	{
		return FALSE;
	}
	LLONG lFindID = 0;
	WRAP_testRecordSetFind_Holiday(loginID, lFindID);
    if (NULL != lFindID)
    {
		int count = GetHolidaysCountRecordSetFind(lFindID);
		CLIENT_FindRecordClose(lFindID);
		return count;
    }
	return -1;
}

BOOL CALL_METHOD WRAP_GetAll_Holidays(int loginID, HolidaysCollection* result)
{
	HolidaysCollection holidaysCollection = {sizeof(HolidaysCollection)};

	LLONG lFinderID = 0;

	NET_IN_FIND_RECORD_PARAM stuIn = {sizeof(stuIn)};
	NET_OUT_FIND_RECORD_PARAM stuOut = {sizeof(stuOut)};
	
	stuIn.emType = NET_RECORD_ACCESSCTLHOLIDAY;
	
	stuIn.pQueryCondition = NULL;
	
	if (CLIENT_FindRecord(loginID, &stuIn, &stuOut, SDK_API_WAITTIME))
	{

		NET_IN_FIND_NEXT_RECORD_PARAM stuIn = {sizeof(stuIn)};
		stuIn.lFindeHandle = lFinderID;
		stuIn.nFileCount = QUERY_COUNT;
	
		NET_OUT_FIND_NEXT_RECORD_PARAM stuOut = {sizeof(stuOut)};
		stuOut.nMaxRecordNum = stuIn.nFileCount;
	
		NET_RECORDSET_ACCESS_CTL_CARDREC stuCardRec[QUERY_COUNT] = {0};
		for (int i = 0; i < sizeof(stuCardRec)/sizeof(stuCardRec[0]); i++)
		{
			stuCardRec[i].dwSize = sizeof(NET_RECORDSET_ACCESS_CTL_CARDREC);
		}
		stuOut.pRecordList = (void*)&stuCardRec[0];
	
		if (CLIENT_FindNextRecord(&stuIn, &stuOut, SDK_API_WAITTIME) >= 0)
		{
			for (int i = 0; i < __min(10, stuOut.nRetRecordNum); i++)
			{
				NET_RECORDSET_HOLIDAY* pHoliday = (NET_RECORDSET_HOLIDAY*)stuOut.pRecordList;
				memcpy(&holidaysCollection.Holidays[i], &pHoliday[i], sizeof(NET_RECORDSET_HOLIDAY));
			}
		}

		CLIENT_FindRecordClose(lFinderID);
	}

	memcpy(result, &holidaysCollection, sizeof(HolidaysCollection));
	return lFinderID != 0;
}