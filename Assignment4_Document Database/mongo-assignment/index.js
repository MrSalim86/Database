const express = require("express");
const { MongoClient } = require("mongodb");
const path = require("path");

const app = express();
const port = 3000;

// MongoDB Connection URL
const uri = "mongodb://localhost:27017";
const client = new MongoClient(uri);
let tweetsCollection;

// Middleware
app.use(express.json());
app.use(express.static("public")); // Serve static files from 'public' folder

// Connect to MongoDB
async function start() {
  try {
    await client.connect();
    const db = client.db("twitterDB");
    tweetsCollection = db.collection("tweets");
    console.log("Connected to MongoDB");

    // Start server after connection
    app.listen(port, () => {
      console.log(`Server listening at http://localhost:${port}`);
    });
  } catch (err) {
    console.error(err);
  }
}

// Get 10 Tweets
app.get("/tweets", async (req, res) => {
  try {
    const tweets = await tweetsCollection.find().limit(10).toArray();
    res.json(tweets);
  } catch (err) {
    console.error(err);
    res.status(500).send("Error fetching tweets");
  }
});

// Insert a New Tweet
app.post("/tweets", async (req, res) => {
  try {
    const text = req.body.text;
    const tweet = {
      text,
      created_at: new Date().toISOString(),
      user: {
        screen_name: "anonymous_user",
      },
      entities: {
        hashtags: [],
      },
    };
    await tweetsCollection.insertOne(tweet);
    res.status(201).send("Tweet inserted");
  } catch (err) {
    console.error(err);
    res.status(500).send("Error inserting tweet");
  }
});

start();
