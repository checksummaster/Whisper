﻿using System;
using System.IO;
using System.Management.Automation;

namespace Whisper
{
	/// <summary>Base class for commands which export results into some text-based format</summary>
	public abstract class ExportBase: Cmdlet
	{
		/// <summary>
		/// <para type="synopsis">Transcribe result produced by <see cref="TranscribeFile" /></para>
		/// <para type="inputType">It requires the value of the correct type</para>
		/// </summary>
		[Parameter( Mandatory = true, ValueFromPipeline = true )]
		public Transcription source { get; set; }

		/// <summary>
		/// <para type="synopsis">Output file to write</para>
		/// </summary>
		[Parameter( Mandatory = true )]
		public string path { get; set; }

		/// <summary>Performs execution of the command</summary>
		protected override void ProcessRecord()
		{
			if( string.IsNullOrEmpty( path ) )
				throw new ArgumentException( "The output path is empty" );
			string dir = Path.GetDirectoryName( path );
			if( !string.IsNullOrEmpty( dir ) )
				Directory.CreateDirectory( dir );

			var results = source.getResult();
			using( var stream = File.CreateText( path ) )
				write( stream, results );
		}

		/// <summary>Actual implementation</summary>
		protected abstract void write( StreamWriter stream, TranscribeResult transcribeResult );
	}
}