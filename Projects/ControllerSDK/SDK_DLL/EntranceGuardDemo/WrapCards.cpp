#include "StdAfx.h"
#include "WrapCards.h"

#include <iostream>
#include <fstream>
using namespace std;

#define QUERY_COUNT	(3)

int CALL_METHOD WRAP_Insert_Card(int lLoginID, NET_RECORDSET_ACCESS_CTL_CARD* stuCard)
{
	if (NULL == lLoginID)
	{
		return 0;
	}

	NET_CTRL_RECORDSET_INSERT_PARAM stuInert = {sizeof(stuInert)};
	stuInert.stuCtrlRecordSetInfo.dwSize = sizeof(NET_CTRL_RECORDSET_INSERT_IN);
    stuInert.stuCtrlRecordSetInfo.emType = NET_RECORD_ACCESSCTLCARD;
	stuInert.stuCtrlRecordSetInfo.pBuf = stuCard;
	stuInert.stuCtrlRecordSetInfo.nBufLen = sizeof(NET_RECORDSET_ACCESS_CTL_CARD);
	
	stuInert.stuCtrlRecordSetResult.dwSize = sizeof(NET_CTRL_RECORDSET_INSERT_OUT);
	
    BOOL bResult = CLIENT_ControlDevice(lLoginID, DH_CTRL_RECORDSET_INSERT, &stuInert, 3000);
	int nRecrdNo = stuInert.stuCtrlRecordSetResult.nRecNo;
	if (bResult)
	{
		return nRecrdNo;
	}
	return 0;
}

BOOL CALL_METHOD WRAP_Update_Card(int lLoginID, NET_RECORDSET_ACCESS_CTL_CARD* stuCard)
{
	if (NULL == lLoginID)
	{
		return FALSE;
	}

	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
    stuInert.emType = NET_RECORD_ACCESSCTLCARD;
	stuInert.pBuf = stuCard;
	stuInert.nBufLen = sizeof(NET_RECORDSET_ACCESS_CTL_CARD);
    BOOL bResult = CLIENT_ControlDevice(lLoginID, DH_CTRL_RECORDSET_UPDATE, &stuInert, SDK_API_WAITTIME);
	return bResult;
}

BOOL CALL_METHOD WRAP_GetCardInfo(int lLoginID, int nRecordNo, NET_RECORDSET_ACCESS_CTL_CARD* result)
{
	if (NULL == lLoginID)
	{
		return FALSE;
	}
	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
	NET_RECORDSET_ACCESS_CTL_CARD stuCard = {sizeof(stuCard)};

	stuCard.nRecNo = nRecordNo;
	stuInert.emType = NET_RECORD_ACCESSCTLCARD;
	stuInert.pBuf = &stuCard;

	int nRet = 0;
	BOOL bRet = CLIENT_QueryDevState(lLoginID, DH_DEVSTATE_DEV_RECORDSET, (char*)&stuInert, sizeof(stuInert), &nRet, 3000);

	memcpy(result, &stuCard, sizeof(stuCard));
	return bRet;
}

BOOL CALL_METHOD WRAP_RemoveCard(int lLoginID, int nRecordNo)
{
	if (NULL == lLoginID)
	{
		return FALSE;
	}
	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
	stuInert.pBuf = &nRecordNo;
	stuInert.nBufLen = sizeof(nRecordNo);
	stuInert.emType = NET_RECORD_ACCESSCTLCARD;
    BOOL bResult = CLIENT_ControlDevice(lLoginID, DH_CTRL_RECORDSET_REMOVE, &stuInert, SDK_API_WAITTIME);
	return bResult;
}

BOOL CALL_METHOD WRAP_RemoveAllCards(int lLoginID)
{
	if (NULL == lLoginID)
	{
		return FALSE;
	}
	NET_CTRL_RECORDSET_PARAM stuInert = {sizeof(stuInert)};
	stuInert.emType = NET_RECORD_ACCESSCTLCARD;
    BOOL bResult = CLIENT_ControlDevice(lLoginID, DH_CTRL_RECORDSET_CLEAR, &stuInert, SDK_API_WAITTIME);
	return bResult;
}

void WRAP_testRecordSetFind_Card(LLONG lLoginId, LLONG& lFinderId, FIND_RECORD_ACCESSCTLCARD_CONDITION stuParam)
{
	NET_IN_FIND_RECORD_PARAM stuIn = {sizeof(stuIn)};
	NET_OUT_FIND_RECORD_PARAM stuOut = {sizeof(stuOut)};
	
	stuIn.emType = NET_RECORD_ACCESSCTLCARD;

	stuIn.pQueryCondition = &stuParam;
	
	if (CLIENT_FindRecord(lLoginId, &stuIn, &stuOut, SDK_API_WAITTIME))
	{
		lFinderId = stuOut.lFindeHandle;
	}
}

void WRAP_testRecordSetFindNext_Card(LLONG lFinderId)
{
	int i = 0, j = 0;
	
	NET_IN_FIND_NEXT_RECORD_PARAM stuIn = {sizeof(stuIn)};
	stuIn.lFindeHandle = lFinderId;
	stuIn.nFileCount = 99999;
	
	NET_OUT_FIND_NEXT_RECORD_PARAM stuOut = {sizeof(stuOut)};
	stuOut.nMaxRecordNum = stuIn.nFileCount;
	
	NET_RECORDSET_ACCESS_CTL_CARD stuCard[99999] = {0};
	for (i = 0; i < sizeof(stuCard)/sizeof(stuCard[0]); i++)
	{
		stuCard[i].dwSize = sizeof(NET_RECORDSET_ACCESS_CTL_CARD);
	}
	stuOut.pRecordList = (void*)&stuCard[0];
	
	if (CLIENT_FindNextRecord(&stuIn, &stuOut, SDK_API_WAITTIME) >= 0)
	{
		printf("CLIENT_FindNextRecord_Card ok!\n");
		
		char szDoorTemp[99999][MAX_NAME_LEN] = {0};
		for (i = 0; i <  __min(99999, stuOut.nRetRecordNum); i++)
		{
			NET_RECORDSET_ACCESS_CTL_CARD* pCard = (NET_RECORDSET_ACCESS_CTL_CARD*)stuOut.pRecordList;
			for (j = 0; j < pCard[i].nDoorNum; j++)
			{
				sprintf(szDoorTemp[i], "%s%d", szDoorTemp[i], pCard[i].sznDoors[j]);
			}
		}
		
		for (j = 0; j < __min(stuOut.nMaxRecordNum, stuOut.nRetRecordNum); j++)
		{
			printf("IsValid:%s, Status:%d, Type:%d, DoorNum:%d, Doors:{%s}, RecNo:%d, TimeSectionNum:%d, UserTimes:%d, CardNo:%s, Pwd:%s, UserID:%s\n", 
				stuCard[j].bIsValid ? "Yes" : "No",
				stuCard[j].emStatus,
				stuCard[j].emType,
				stuCard[j].nDoorNum,
				szDoorTemp[j],
				stuCard[j].nRecNo,
				stuCard[j].nTimeSectionNum,
				stuCard[j].nUserTime,
				stuCard[j].szCardNo,
				stuCard[j].szPsw,
				stuCard[j].szUserID);
		}
	}
	else
	{
		printf("CLIENT_FindNextRecord_Card failed:0x%08x!\n", CLIENT_GetLastError());
	}
}

int GetCardsCountRecordSetFind(LLONG& lFinderId)
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

int CALL_METHOD WRAP_Get_CardsCount(int lLoginID, FIND_RECORD_ACCESSCTLCARD_CONDITION* stuParam)
{
	if (NULL == lLoginID)
	{
		return -1;
	}
	LLONG lFindID = 0;

	WRAP_testRecordSetFind_Card(lLoginID, lFindID, *stuParam);
    if (NULL != lFindID)
    {
		int count = GetCardsCountRecordSetFind(lFindID);
		CLIENT_FindRecordClose(lFindID);
		return count;
    }
	return -1;
}

BOOL CALL_METHOD WRAP_GetAllCards(int lLoginId, CardsCollection* result)
 {
 	CardsCollection cardsCollection = {sizeof(CardsCollection)};
 
 	LLONG lFinderID = 0;
 
 	FIND_RECORD_ACCESSCTLCARD_CONDITION stuParam = {sizeof(stuParam)};
 	stuParam.bIsValid = TRUE;
 	strcpy(stuParam.szCardNo, "1");
 	strcpy(stuParam.szUserID, "1");
 
 	WRAP_testRecordSetFind_Card(lLoginId, lFinderID, stuParam);
 	if (lFinderID != 0)
 	{
 		int i = 0, j = 0;
 	
 		NET_IN_FIND_NEXT_RECORD_PARAM stuIn = {sizeof(stuIn)};
 		stuIn.lFindeHandle = lFinderID;
 		stuIn.nFileCount = 500;
 	
 		NET_OUT_FIND_NEXT_RECORD_PARAM stuOut = {sizeof(stuOut)};
 		stuOut.nMaxRecordNum = stuIn.nFileCount;
 	
 		NET_RECORDSET_ACCESS_CTL_CARD stuCard[500] = {0};
 		for (i = 0; i < sizeof(stuCard)/sizeof(stuCard[0]); i++)
 		{
 			stuCard[i].dwSize = sizeof(NET_RECORDSET_ACCESS_CTL_CARD);
 		}
 		stuOut.pRecordList = (void*)&stuCard[0];
 	
 		if (CLIENT_FindNextRecord(&stuIn, &stuOut, SDK_API_WAITTIME) >= 0)
 		{
 			cardsCollection.Count = stuOut.nRetRecordNum;
 			char szDoorTemp[500][MAX_NAME_LEN] = {0};
 			for (i = 0; i < __min(500, stuOut.nRetRecordNum); i++)
 			{
 				NET_RECORDSET_ACCESS_CTL_CARD* pCard = (NET_RECORDSET_ACCESS_CTL_CARD*)stuOut.pRecordList;
 				memcpy(&cardsCollection.Cards[i], &pCard[i], sizeof(NET_RECORDSET_ACCESS_CTL_CARD));
 			}
 		}

		CLIENT_FindRecordClose(lFinderID);
 	}
 
 	memcpy(result, &cardsCollection, sizeof(CardsCollection));
 	return lFinderID != 0;
 }