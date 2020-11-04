﻿using System;
namespace MysteryLocation
{
    public class Post
    {
        private int id { get; set; }
        private String subject { get; set; }
        private String body { get; set; }
        private String created { get; set; }
        private String lastUpdated { get; set; }
        private Coordinate position { get; set; }

public Post(int id, String subject, String body, String image, String created, String lastUpdated, Coordinate position)
        {
            this.id = id;
            this.subject = subject;
            this.body = body;
            this.created = created;
            this.lastUpdated = lastUpdated;
            this.position = position;

        }

        public int getId()
        {
            return id;
        }


        public void unlockImage()
        {

        }

        public void lockImage()
        {

        }

        public String toString()
        {
            string toReturn = id + "\n" + subject + "\n" + body + "\n" + created + "\n" + lastUpdated + "\n" + position.toString() + "\n";
            return toReturn;
        }
    }
}