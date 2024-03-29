﻿using System;
using System.IO;


namespace MysteryLocation.Model
{
    public class PostAttachment
    {
        public int obsID { get; set; }
        public string description { get; set; }

        public PostAttachment(int obsID, byte[] bytes)
        {
            this.obsID = obsID;
            Stream imgStream = new MemoryStream(bytes);
            convertImageSourceToBase64(imgStream);
        }

        private void convertImageSourceToBase64(Stream x)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                x.CopyTo(ms);
                description = Convert.ToBase64String(ms.ToArray());
            }
        }

    }
}