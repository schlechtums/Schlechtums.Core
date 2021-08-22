using Schlechtums.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Schlechtums.Core.Common
{
	/// <summary>
	/// Restores a file's contents when disposed
	/// </summary>
	public class TemporaryFileWithRestore : TemporaryFile
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">The file to modify</param>
		public TemporaryFileWithRestore(String file)
			: this(file, null)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">The file to modify</param>
		/// <param name="content">The content to write to</param>
		public TemporaryFileWithRestore(String file, Byte[] content)
		{
			this.m_File = Path.GetFullPath(file);
			this.m_OriginalContent = File.ReadAllBytes(this.m_File);

			if (content != null)
				this.WriteAllBytes(content);
		}

		private Byte[] m_OriginalContent;

		public override void Dispose()
		{
			File.WriteAllBytes(this.m_File, this.m_OriginalContent);
		}

		new public TemporaryFileWithRestore WithExtension(String extension)
		{
			this.m_File += extension.EnsureStartsWith(".");
			return this;
		}
	}

	/// <summary>
	/// Creates a file and deletets it when disposed
	/// </summary>
	public class TemporaryFile : IDisposable
	{
		#region <<< Static Helpers >>>
		/// <summary>
		/// Creates a TemporaryFile with the given content.
		/// </summary>
		/// <param name="content">Content as a string</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(String content)
		{
			var tf = new TemporaryFile();
			tf.WriteAllText(content);
			return tf;
		}

		/// <summary>
		/// Creates a TemporaryFile with the given content and desired extension.
		/// </summary>
		/// <param name="content">Content as a string</param>
		/// <param name="ext">The desired extension.</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(String content, String ext)
		{
			var tf = new TemporaryFile().WithExtension(ext);
			tf.WriteAllText(content);
			return tf;
		}

		/// <summary>
		/// Creates a TemporaryFile with the given content.
		/// </summary>
		/// <param name="content">Content as a byte array.</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(Byte[] content)
		{
			var tf = new TemporaryFile();
			tf.WriteAllBytes(content);
			return tf;
		}

		/// <summary>
		/// Creates a TemporaryFile with the given content and desired extension.
		/// </summary>
		/// <param name="content">Content as a byte array.</param>
		/// <param name="ext">The desired extension.</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(Byte[] content, String ext)
		{
			var tf = new TemporaryFile().WithExtension(ext);
			tf.WriteAllBytes(content);
			return tf;
		}

		/// <summary>
		/// Creates a TemporaryFile with the given content.
		/// </summary>
		/// <param name="content">Content as an enumeration of strings.</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(IEnumerable<String> content)
		{
			var tf = new TemporaryFile();
			tf.WriteAllLines(content.ToArray());
			return tf;
		}

		/// <summary>
		/// Creates a TemporaryFile with the given content and desired extension.
		/// </summary>
		/// <param name="content">Content as an enumeration of strings.</param>
		/// <param name="ext">The desired extension.</param>
		/// <returns>The TemporaryFile</returns>
		public static TemporaryFile WithContent(IEnumerable<String> content, String ext)
		{
			var tf = new TemporaryFile().WithExtension(ext);
			tf.WriteAllLines(content.ToArray());
			return tf;
		}

		public static TemporaryFile WithContent(Object content)
		{
			return TemporaryFile.FromObjectContentHelper(new TemporaryFile(), content);
		}

		public static TemporaryFile WithContent(Object content, String ext)
		{
			return TemporaryFile.FromObjectContentHelper(new TemporaryFile().WithExtension(ext), content);
		}

		private static TemporaryFile FromObjectContentHelper(TemporaryFile tf, Object content)
		{
			var bytes = content as Byte[];
			if (bytes != null)
				tf.WriteAllBytes(bytes);
			else
			{
				var @string = content as String;
				if (@string != null)
					tf.WriteAllText(content as String);
				else
					tf.WriteAllLines((content as IEnumerable<String>).ToArray());
			}

			return tf;
		}
		#endregion

		public TemporaryFile()
			: this(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()))
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="throwExceptionOnDelete">True/False to throw an exception if one happens when disposing</param>
		public TemporaryFile(Boolean throwExceptionOnDelete)
			: this(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), throwExceptionOnDelete)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">A specific name for the temporary file</param>
		public TemporaryFile(String fileName)
			: this(fileName, false)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">A specific name for the temporary file</param>
		/// <param name="throwExceptionOnDelete">True/False to throw an exception if one happens when disposing</param>
		public TemporaryFile(String fileName, Boolean throwExceptionOnDelete)
		{
			this.m_File = Path.GetFullPath(fileName);
			this.m_ThrowExceptionOnDelete = throwExceptionOnDelete;
		}

		protected String m_File;
		protected Boolean m_ThrowExceptionOnDelete;

		public String FullPath { get { return this.m_File; } }

		public virtual void Dispose()
		{
			if (!this.m_File.IsNullOrWhitespace() && File.Exists(this.m_File))
			{
				try
				{
					File.Delete(this.m_File);
				}
				catch
				{
					if (this.m_ThrowExceptionOnDelete)
						throw;
				}
			}
		}

		#region <<< Static System.IO.File methods >>>
		public void AppendAllText(String contents)
		{
			this.WriteAllText(this.ReadAllText() + contents);
		}

		public void AppendAllText(String contents, Encoding encoding)
		{
			this.WriteAllText(this.ReadAllText() + contents, encoding);
		}

		public void AppendAllLines(IEnumerable<String> contents)
		{
			var lines = this.ReadAllLines().ToList();
			lines.AddRange(contents);
			this.WriteAllLines(lines.ToArray());
		}

		public void AppendAllLines(IEnumerable<String> contents, Encoding encoding)
		{
			var lines = this.ReadAllLines().ToList();
			lines.AddRange(contents);
			this.WriteAllLines(lines.ToArray(), encoding);
		}

		public StreamWriter AppendText()
		{
			return File.AppendText(this.m_File);
		}

		public void Copy(String destFileName)
		{
			File.Copy(this.m_File, destFileName);
		}

		public void Copy(String destFileName, Boolean overwrite)
		{
			File.Copy(this.m_File, destFileName, overwrite);
		}

		public void Decrypt()
		{
			File.Decrypt(this.m_File);
		}

		public void Encrypt()
		{
			File.Encrypt(this.m_File);
		}

		public Boolean Exists()
		{
			return File.Exists(this.m_File);
		}

		public FileAttributes GetAttributes()
		{
			return File.GetAttributes(this.m_File);
		}

		public DateTime GetCreationTime()
		{
			return File.GetCreationTime(this.m_File);
		}

		public DateTime GetCreationTimeUtc()
		{
			return File.GetCreationTimeUtc(this.m_File);
		}

		public DateTime GetLastAccessTime()
		{
			return File.GetLastAccessTime(this.m_File);
		}

		public DateTime GetLastAccessTimeUtc()
		{
			return File.GetLastAccessTimeUtc(this.m_File);
		}

		public DateTime GetLastWriteTime()
		{
			return File.GetLastWriteTime(this.m_File);
		}

		public DateTime GetLastWriteTimeUtc()
		{
			return File.GetLastWriteTimeUtc(this.m_File);
		}

		public void Move(string destFileName)
		{
			File.Move(this.m_File, destFileName);
		}

		public FileStream Open(FileMode mode)
		{
			return File.Open(this.m_File, mode);
		}

		public FileStream Open(FileMode mode, FileAccess access)
		{
			return File.Open(this.m_File, mode, access);
		}

		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return File.Open(this.m_File, mode, access, share);
		}

		public FileStream OpenRead()
		{
			return File.OpenRead(this.m_File);
		}

		public StreamReader OpenText()
		{
			return File.OpenText(this.m_File);
		}

		public FileStream OpenWrite()
		{
			return File.OpenWrite(this.m_File);
		}

		public String TryReadAllText()
		{
			if (this.Exists())
				return this.ReadAllText();
			else
				return null;
		}

		public virtual String ReadAllText()
		{
			return File.ReadAllText(this.m_File);
		}

		public String TryReadAllText(Encoding encoding)
		{
			if (this.Exists())
				return this.ReadAllText(encoding);
			else
				return null;
		}

		public virtual String ReadAllText(Encoding encoding)
		{
			return File.ReadAllText(this.m_File, encoding);
		}

		public XElement TryReadAsXml()
		{
			if (this.Exists())
				return this.ReadAsXml();
			else
				return null;
		}

		public XElement ReadAsXml()
		{
			return this.ReadAllText().ToXElement();
		}

		public XElement TryReadAsXml(Encoding encoding)
		{
			if (this.Exists())
				return this.ReadAsXml(encoding);
			else
				return null;
		}

		public XElement ReadAsXml(Encoding encoding)
		{
			return this.ReadAllText(encoding).ToXElement();
		}

		public String[] TryReadAllLines()
		{
			if (this.Exists())
				return this.ReadAllLines();
			else
				return null;
		}

		public virtual String[] ReadAllLines()
		{
			return File.ReadAllLines(this.m_File);
		}

		public String[] TryReadAllLines(Encoding encoding)
		{
			if (this.Exists())
				return this.ReadAllLines(encoding);
			else
				return null;
		}

		public virtual String[] ReadAllLines(Encoding encoding)
		{
			return File.ReadAllLines(this.m_File, encoding);
		}

		public Byte[] TryReadAllBytes()
		{
			if (this.Exists())
				return this.ReadAllBytes();
			else
				return null;
		}

		public virtual Byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.m_File);
		}

		public void Replace(String destinationFileName, String destinationBackupFileName)
		{
			File.Replace(this.m_File, destinationFileName, destinationBackupFileName);
		}

		public void Replace(String destinationFileName, String destinationBackupFilename, Boolean ignoreMetadataErrors)
		{
			File.Replace(this.m_File, destinationFileName, destinationBackupFilename, ignoreMetadataErrors);
		}

		public void SetAttributes(FileAttributes fileAttributes)
		{
			File.SetAttributes(this.m_File, fileAttributes);
		}

		public void SetCreationTime(DateTime creationTime)
		{
			File.SetCreationTime(this.m_File, creationTime);
		}

		public void SetCreationTimeUtc(DateTime creationTimeUtc)
		{
			File.SetCreationTimeUtc(this.m_File, creationTimeUtc);
		}

		public void SetLastAccessTime(DateTime lastAccessTime)
		{
			File.SetLastAccessTime(this.m_File, lastAccessTime);
		}

		public void SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
		{
			File.SetLastAccessTimeUtc(this.m_File, lastAccessTimeUtc);
		}

		public void SetLastWriteTime(DateTime lastWriteTime)
		{
			File.SetLastWriteTime(this.m_File, lastWriteTime);
		}

		public void SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
		{
			File.SetLastWriteTimeUtc(this.m_File, lastWriteTimeUtc);
		}

		public virtual void WriteAllText(String contents)
		{
			File.WriteAllText(this.m_File, contents);
		}

		public virtual void WriteAllText(String contents, Encoding enc)
		{
			File.WriteAllText(this.m_File, contents, enc);
		}

		public virtual void WriteAllLines(String[] contents)
		{
			File.WriteAllLines(this.m_File, contents);
		}

		public virtual void WriteAllLines(String[] contents, Encoding enc)
		{
			File.WriteAllLines(this.m_File, contents, enc);
		}

		public virtual void WriteAllBytes(Byte[] bytes)
		{
			File.WriteAllBytes(this.m_File, bytes);
		}
		#endregion

		public static implicit operator String(TemporaryFile tf)
		{
			return tf.ToString();
		}

		public override string ToString()
		{
			return this.m_File;
		}

		public TemporaryFile WithExtension(String extension)
		{
			var fSplit = this.m_File.Split(".");

			this.m_File = fSplit.Take(fSplit.Length - 1).Join(".") + extension.EnsureStartsWith(".");
			return this;
		}

		public Boolean IsLocked()
		{
			return new FileInfo(this.m_File).IsLocked();
		}

		public String Filename
		{
			get
			{
				return Path.GetFileName(this.m_File);
			}
		}
	}
}