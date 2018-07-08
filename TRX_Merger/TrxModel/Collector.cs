using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.TrxModel
{
	public class Collector
	{
		public string AgentName { get; set; }
		public string Uri { get; set; }
		public string CollectorDisplayName { get; set; }
		public List<UriAttachment> UriAttachments { get; set; }
	}
}
