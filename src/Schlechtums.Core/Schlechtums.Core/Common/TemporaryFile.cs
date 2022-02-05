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
		public TemporaryFileWithRestore(string file)
			: this(file, null)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file">The file to modify</param>
		/// <param name="content">The content to write to</param>
		public TemporaryFileWithRestore(string file, byte[] content)
		{
			this.file = Path.GetFullPath(file);
			this.originalContent = File.ReadAllBytes(this.file);

			if (content != null)
				this.WriteAllBytes(content);
		}

		private byte[] originalContent;

		public override void Dispose()
		{
			File.WriteAllBytes(this.file, this.originalContent);
		}

		new public TemporaryFileWithRestore WithExtension(string extension)
		{
			this.file += extension.EnsureStartsWith(".");
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
		public static TemporaryFile WithContent(string content)
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
		public static TemporaryFile WithContent(string content, string ext)
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
		public static TemporaryFile WithContent(byte[] content)
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
		public static TemporaryFile WithContent(byte[] content, string ext)
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
		public static TemporaryFile WithContent(IEnumerable<string> content)
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
		public static TemporaryFile WithContent(IEnumerable<string> content, string ext)
		{
			var tf = new TemporaryFile().WithExtension(ext);
			tf.WriteAllLines(content.ToArray());
			return tf;
		}

		public static TemporaryFile WithContent(Object content)
		{
			return TemporaryFile.FromObjectContentHelper(new TemporaryFile(), content);
		}

		public static TemporaryFile WithContent(Object content, string ext)
		{
			return TemporaryFile.FromObjectContentHelper(new TemporaryFile().WithExtension(ext), content);
		}

		private static TemporaryFile FromObjectContentHelper(TemporaryFile tf, Object content)
		{
			var bytes = content as byte[];
			if (bytes != null)
				tf.WriteAllBytes(bytes);
			else
			{
				var @string = content as String;
				if (@string != null)
					tf.WriteAllText(content as String);
				else
					tf.WriteAllLines((content as IEnumerable<string>).ToArray());
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
		public TemporaryFile(bool throwExceptionOnDelete)
			: this(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), throwExceptionOnDelete)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">A specific name for the temporary file</param>
		public TemporaryFile(string fileName)
			: this(fileName, false)
		{ }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName">A specific name for the temporary file</param>
		/// <param name="throwExceptionOnDelete">True/False to throw an exception if one happens when disposing</param>
		public TemporaryFile(string fileName, bool throwExceptionOnDelete)
		{
			this.file = Path.GetFullPath(fileName);
			this.throwExceptionOnDelete = throwExceptionOnDelete;
		}

		protected string file;
		protected bool throwExceptionOnDelete;

		public string FullPath { get { return this.file; } }

		public virtual void Dispose()
		{
			if (!this.file.IsNullOrWhitespace() && File.Exists(this.file))
			{
				try
				{
					File.Delete(this.file);
				}
				catch
				{
					if (this.throwExceptionOnDelete)
						throw;
				}
			}
		}

		#region <<< Static System.IO.File methods >>>
		public void AppendAllText(string contents)
		{
			this.WriteAllText(this.ReadAllText() + contents);
		}

		public void AppendAllText(string contents, Encoding encoding)
		{
			this.WriteAllText(this.ReadAllText() + contents, encoding);
		}

		public void AppendAllLines(IEnumerable<string> contents)
		{
			var lines = this.ReadAllLines().ToList();
			lines.AddRange(contents);
			this.WriteAllLines(lines.ToArray());
		}

		public void AppendAllLines(IEnumerable<string> contents, Encoding encoding)
		{
			var lines = this.ReadAllLines().ToList();
			lines.AddRange(contents);
			this.WriteAllLines(lines.ToArray(), encoding);
		}

		public StreamWriter AppendText()
		{
			return File.AppendText(this.file);
		}

		public void Copy(string destFileName)
		{
			File.Copy(this.file, destFileName);
		}

		public void Copy(string destFileName, bool overwrite)
		{
			File.Copy(this.file, destFileName, overwrite);
		}

		public void Decrypt()
		{
			File.Decrypt(this.file);
		}

		public void Encrypt()
		{
			File.Encrypt(this.file);
		}

		public bool Exists()
		{
			return File.Exists(this.file);
		}

		public FileAttributes GetAttributes()
		{
			return File.GetAttributes(this.file);
		}

		public DateTime GetCreationTime()
		{
			return File.GetCreationTime(this.file);
		}

		public DateTime GetCreationTimeUtc()
		{
			return File.GetCreationTimeUtc(this.file);
		}

		public DateTime GetLastAccessTime()
		{
			return File.GetLastAccessTime(this.file);
		}

		public DateTime GetLastAccessTimeUtc()
		{
			return File.GetLastAccessTimeUtc(this.file);
		}

		public DateTime GetLastWriteTime()
		{
			return File.GetLastWriteTime(this.file);
		}

		public DateTime GetLastWriteTimeUtc()
		{
			return File.GetLastWriteTimeUtc(this.file);
		}

		public void Move(string destFileName)
		{
			File.Move(this.file, destFileName);
		}

		public FileStream Open(FileMode mode)
		{
			return File.Open(this.file, mode);
		}

		public FileStream Open(FileMode mode, FileAccess access)
		{
			return File.Open(this.file, mode, access);
		}

		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return File.Open(this.file, mode, access, share);
		}

		public FileStream OpenRead()
		{
			return File.OpenRead(this.file);
		}

		public StreamReader OpenText()
		{
			return File.OpenText(this.file);
		}

		public FileStream OpenWrite()
		{
			return File.OpenWrite(this.file);
		}

		public string TryReadAllText()
		{
			if (this.Exists())
				return this.ReadAllText();
			else
				return null;
		}

		public virtual string ReadAllText()
		{
			return File.ReadAllText(this.file);
		}

		public string TryReadAllText(Encoding encoding)
		{
			if (this.Exists())
				return this.ReadAllText(encoding);
			else
				return null;
		}

		public virtual string ReadAllText(Encoding encoding)
		{
			return File.ReadAllText(this.file, encoding);
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

		public string[] TryReadAllLines()
		{
			if (this.Exists())
				return this.ReadAllLines();
			else
				return null;
		}

		public virtual string[] ReadAllLines()
		{
			return File.ReadAllLines(this.file);
		}

		public string[] TryReadAllLines(Encoding encoding)
		{
			if (this.Exists())
				return this.ReadAllLines(encoding);
			else
				return null;
		}

		public virtual string[] ReadAllLines(Encoding encoding)
		{
			return File.ReadAllLines(this.file, encoding);
		}

		public byte[] TryReadAllBytes()
		{
			if (this.Exists())
				return this.ReadAllBytes();
			else
				return null;
		}

		public virtual byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.file);
		}

		public void Replace(string destinationFileName, string destinationBackupFileName)
		{
			File.Replace(this.file, destinationFileName, destinationBackupFileName);
		}

		public void Replace(string destinationFileName, string destinationBackupFilename, bool ignoreMetadataErrors)
		{
			File.Replace(this.file, destinationFileName, destinationBackupFilename, ignoreMetadataErrors);
		}

		public void SetAttributes(FileAttributes fileAttributes)
		{
			File.SetAttributes(this.file, fileAttributes);
		}

		public void SetCreationTime(DateTime creationTime)
		{
			File.SetCreationTime(this.file, creationTime);
		}

		public void SetCreationTimeUtc(DateTime creationTimeUtc)
		{
			File.SetCreationTimeUtc(this.file, creationTimeUtc);
		}

		public void SetLastAccessTime(DateTime lastAccessTime)
		{
			File.SetLastAccessTime(this.file, lastAccessTime);
		}

		public void SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
		{
			File.SetLastAccessTimeUtc(this.file, lastAccessTimeUtc);
		}

		public void SetLastWriteTime(DateTime lastWriteTime)
		{
			File.SetLastWriteTime(this.file, lastWriteTime);
		}

		public void SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
		{
			File.SetLastWriteTimeUtc(this.file, lastWriteTimeUtc);
		}

		public virtual void WriteAllText(string contents)
		{
			File.WriteAllText(this.file, contents);
		}

		public virtual void WriteAllText(string contents, Encoding enc)
		{
			File.WriteAllText(this.file, contents, enc);
		}

		public virtual void WriteAllLines(string[] contents)
		{
			File.WriteAllLines(this.file, contents);
		}

		public virtual void WriteAllLines(string[] contents, Encoding enc)
		{
			File.WriteAllLines(this.file, contents, enc);
		}

		public virtual void WriteAllBytes(byte[] bytes)
		{
			File.WriteAllBytes(this.file, bytes);
		}
		#endregion

		public static implicit operator string(TemporaryFile tf)
		{
			return tf.ToString();
		}

		public override string ToString()
		{
			return this.file;
		}

		public TemporaryFile WithExtension(string extension)
		{
			var fSplit = this.file.Split(".");

			this.file = fSplit.Take(fSplit.Length - 1).Join(".") + extension.EnsureStartsWith(".");
			return this;
		}

		public bool IsLocked()
		{
			return new FileInfo(this.file).IsLocked();
		}

		public string Filename
		{
			get
			{
				return Path.GetFileName(this.file);
			}
		}
	}
}