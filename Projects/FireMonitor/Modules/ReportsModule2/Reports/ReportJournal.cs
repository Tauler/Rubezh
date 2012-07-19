﻿using System;
using System.Collections.Generic;
//using CrystalDecisions.CrystalReports.Engine;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using JournalModule.ViewModels;
using ReportsModule2.Models;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Xps.Packaging;
using ReportsModule2.DocumentPaginatorModel;
using System.IO;
using System.Text;

namespace ReportsModule2.Reports
{
	public class ReportJournal : BaseReportGeneric<ReportJournalModel>
	{
		public ReportJournal()
		{
			base.ReportFileName = "JournalCrystalReport.rpt";
			ReportArchiveFilter = new ReportArchiveFilter();
			DataTable = new Table();
		}

		public ReportJournal(ArchiveFilterViewModel archiveFilterViewModel)
		{
			base.ReportFileName = "JournalCrystalReport.rpt";
			ReportArchiveFilter = new ReportArchiveFilter(archiveFilterViewModel);
			DataTable = new Table();
		}

		public DateTime EndDate { get; set; }
		public DateTime StartDate { get; set; }
		public ReportArchiveFilter ReportArchiveFilter { get; set; }
		public Table DataTable { get; set; }
		public XpsDocument XpsDocument
		{
			get
			{
				var xpsDocument = new XpsDocument("journal.xps", FileAccess.Read);
				return xpsDocument;
			}
		}
		public StringBuilder FlowDocumentStringBuilder { get; set; }

		public override void LoadData()
		{
			DataList = new List<ReportJournalModel>();
			foreach (var journalRecord in ReportArchiveFilter.JournalRecords)
			{
				DataList.Add(new ReportJournalModel()
				{
					DeviceTime = journalRecord.DeviceTime.ToString(),
					SystemTime = journalRecord.SystemTime.ToString(),
					ZoneName = journalRecord.ZoneName,
					Description = journalRecord.Description,
					Device = journalRecord.DeviceName,
					Panel = journalRecord.PanelName,
					User = journalRecord.User
				});
			}
			StartDate = ReportArchiveFilter.StartDate;
			EndDate = ReportArchiveFilter.EndDate;
			//LoadDataTable();
		}

		void LoadDataTable()
		{
			DataTable = new Table();
			int numberOfColumns = 7;
			for (int i = 0; i < numberOfColumns; i++)
			{
				DataTable.Columns.Add(new TableColumn());
			}

			DataTable.BorderBrush = Brushes.White;
			DataTable.BorderThickness = new Thickness(1);
			DataTable.CellSpacing = 0.1;
			DataTable.RowGroups.Add(new TableRowGroup());
			DataTable.RowGroups[0].Rows.Add(new TableRow());
			var currentRow = DataTable.RowGroups[0].Rows[0];
			currentRow.Background = Brushes.Silver;
			currentRow.FontSize = 14;
			currentRow.FontWeight = FontWeights.Bold;
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Время устройства"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Системное время"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Зона"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Событие"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Устройство датчик"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Устройство"))));
			currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Пользователь"))));

			DataTable.RowGroups.Add(new TableRowGroup());
			DataTable.RowGroups[1].FontSize = 12;
			DataTable.RowGroups[1].FontWeight = FontWeights.Normal;
			DataTable.RowGroups[1].Background = Brushes.White;
			int rowNumber = 0;
			foreach (var journalModel in DataList)
			{
				DataTable.RowGroups[1].Rows.Add(new TableRow());
				currentRow = DataTable.RowGroups[1].Rows[rowNumber];
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.DeviceTime))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.SystemTime))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.ZoneName))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.Description))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.Device))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.Panel))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				currentRow.Cells.Add(new TableCell(new Paragraph(new Run(journalModel.User))) { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) });
				rowNumber++;
			}

		}

		public void CreateXpsDocument()
		{
			var flowDocument = new FlowDocument();
			flowDocument.Blocks.Add(DataTable);
			ConvertFlowToXPS.SaveFlowAsXpsInFile(flowDocument);
		}

		public void SaveFlowDocumentToTxt()
		{
			var flowDocument = new FlowDocument();
			flowDocument.Blocks.Add(DataTable);
			ConvertFlowToXPS.SaveFlowAsString(flowDocument);
		}

		public void CreateFlowDocumentStringBuilder()
		{
			var flowDocumentSB = new StringBuilder();
			flowDocumentSB.Append(@"<FlowDocument xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">");
			flowDocumentSB.Append(@"<Table CellSpacing=""0.1"" BorderThickness=""1,1,1,1"" BorderBrush=""#FFFFFFFF"">");
			flowDocumentSB.Append(@"<Table.Columns><TableColumn /><TableColumn /><TableColumn /><TableColumn /><TableColumn /><TableColumn /><TableColumn /></Table.Columns>");
			flowDocumentSB.Append(@"<TableRowGroup><TableRow FontWeight=""Bold"" FontSize=""14"" Background=""#FFC0C0C0""><TableCell><Paragraph>Время устройства</Paragraph></TableCell><TableCell><Paragraph>Системное время</Paragraph></TableCell><TableCell><Paragraph>Зона</Paragraph></TableCell><TableCell><Paragraph>Событие</Paragraph></TableCell><TableCell><Paragraph>Устройство датчик</Paragraph></TableCell><TableCell><Paragraph>Устройство</Paragraph></TableCell><TableCell><Paragraph>Пользователь</Paragraph></TableCell></TableRow></TableRowGroup>");
			flowDocumentSB.Append(@"<TableRowGroup FontWeight=""Normal"" FontSize=""12"" Background=""#FFFFFFFF"">");
			foreach (var journalModel in DataList)
			{
				for (int i = 0; i < 1; i++)
				{
					string tableCellHeader = @"<TableCell BorderThickness=""1,1,1,1"" BorderBrush=""#FF000000"">";
					flowDocumentSB.Append(@"<TableRow>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.DeviceTime.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.SystemTime.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.ZoneName.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.Description.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.Device.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.Panel.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(tableCellHeader + "<Paragraph>" + journalModel.User.ToString() + "</Paragraph></TableCell>");
					flowDocumentSB.Append(@"</TableRow>");
				}
			}
			flowDocumentSB.Append(@"</TableRowGroup></Table></FlowDocument>");
			FlowDocumentStringBuilder = flowDocumentSB;
		}
	}

	public class ReportArchiveFilter
	{
		public ReportArchiveFilter()
		{
			SetFilter();
			Initialize();
		}

		public ReportArchiveFilter(ArchiveFilterViewModel archiveFilterViewModel)
		{
			SetFilter(archiveFilterViewModel);
			Initialize();
		}

		void Initialize()
		{
			JournalRecords = new List<JournalRecord>();
			LoadArchive();
		}

		public readonly DateTime ArchiveFirstDate = FiresecManager.FiresecService.GetArchiveStartDate().Result;
		public List<JournalRecord> JournalRecords { get; set; }
		public ArchiveFilter ArchiveFilter { get; set; }
		public bool IsFilterOn { get; set; }
		public DateTime StartDate { get; private set; }
		public DateTime EndDate { get; private set; }

		void SetFilter(ArchiveFilterViewModel archiveFilterViewModel)
		{
			ArchiveFilter = archiveFilterViewModel.GetModel();
			StartDate = archiveFilterViewModel.StartDate;
			EndDate = archiveFilterViewModel.EndDate;
		}

		void SetFilter()
		{
			var archiveFilter = new ArchiveFilter() { StartDate = ArchiveFirstDate, EndDate = DateTime.Now, UseSystemDate = false };
			var archiveFilterViewModel = new ArchiveFilterViewModel(archiveFilter);
			ArchiveFilter = archiveFilterViewModel.GetModel();
			StartDate = archiveFilterViewModel.StartDate;
			EndDate = archiveFilterViewModel.EndDate;
		}

		public void LoadArchive()
		{
			JournalRecords = new List<JournalRecord>();
			OperationResult<List<JournalRecord>> operationResult;
			operationResult = FiresecManager.FiresecService.GetFilteredArchive(ArchiveFilter);
			if (operationResult.HasError == false)
			{
				foreach (var journalRecord in operationResult.Result)
				{
					JournalRecords.Add(journalRecord);
				}
			}
		}
	}
}