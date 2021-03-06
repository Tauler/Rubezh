#include "StdAfx.h"
#include "WrapTimeShedules.h"

#include <iostream>
#include <fstream>
using namespace std;

#define QUERY_COUNT	(3)

BOOL CALL_METHOD WRAP_GetTimeSchedule(int loginID, int index, CFG_ACCESS_TIMESCHEDULE_INFO* result)
{	
	char szJsonBuf[1024 * 40] = {0};
	int nerror = 0;
	BOOL bRet = CLIENT_GetNewDevConfig(loginID, CFG_CMD_ACCESSTIMESCHEDULE, index, szJsonBuf, sizeof(szJsonBuf), &nerror, SDK_API_WAITTIME);
	int nRetLen = 0;
	CFG_ACCESS_TIMESCHEDULE_INFO stuInfo = {0};
	bRet = CLIENT_ParseData(CFG_CMD_ACCESSTIMESCHEDULE, szJsonBuf, &stuInfo, sizeof(stuInfo), &nRetLen);
	memcpy(result, &stuInfo, sizeof(stuInfo));
	return bRet;
}

BOOL CALL_METHOD WRAP_SetTimeSchedule(int loginID, int index, CFG_ACCESS_TIMESCHEDULE_INFO* param)
{	
	char szJsonBufSet[1024 * 40] = {0};

	BOOL bRet = CLIENT_PacketData(CFG_CMD_ACCESSTIMESCHEDULE, param, sizeof(CFG_ACCESS_TIMESCHEDULE_INFO), szJsonBufSet, sizeof(szJsonBufSet));
	int nerror = 0;
	int nrestart = 0;
	bRet = CLIENT_SetNewDevConfig(loginID, CFG_CMD_ACCESSTIMESCHEDULE, index, szJsonBufSet, sizeof(szJsonBufSet), &nerror, &nrestart, SDK_API_WAITTIME);
	return bRet;
}

//ofstream myfile;
//myfile.open ("D://SDKOutput.txt");
//myfile << x << "\n";
//myfile << "\n";
//myfile.close();
