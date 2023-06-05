package com.um.feri.direct4me.android.api

import com.um.feri.direct4me.android.ui.history.PostboxHistoryItem
import okhttp3.OkHttpClient
import okhttp3.Request
import org.json.JSONArray
import java.text.SimpleDateFormat
import java.util.*

object ApiClient {
    private val client = OkHttpClient()

    private const val BASE_URL = "https://localhost:44322"

    fun getUserHistory(userGuid: String): List<PostboxHistoryItem> {
        val url = "$BASE_URL/postboxes/history/user/$userGuid"
        val request = Request.Builder()
            .url(url)
            .build()

        val response = client.newCall(request).execute()
        val responseBody = response.body?.string()

        val userHistory = mutableListOf<PostboxHistoryItem>()

        if (response.isSuccessful && !responseBody.isNullOrBlank()) {
            val jsonArray = JSONArray(responseBody)
            for (i in 0 until jsonArray.length()) {
                val jsonObject = jsonArray.getJSONObject(i)
                val date = jsonObject.getString("Date")
                val userName = jsonObject.getString("UserName")
                val postboxId = jsonObject.getString("PostboxId")
                val type = jsonObject.optString("Type", null.toString())
                val success = jsonObject.getBoolean("Success")

                val dateFormatter =
                    SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault())
                val parsedDate = dateFormatter.parse(date)

                if (parsedDate != null) {
                    val historyItem =
                        PostboxHistoryItem(parsedDate, userName, postboxId, type, success)
                    userHistory.add(historyItem)
                }
            }
        }

        return userHistory
    }

    fun getPostboxHistory(boxGuid: String): List<PostboxHistoryItem> {
        val url = "$BASE_URL/postboxes/history/box/$boxGuid"
        val request = Request.Builder()
            .url(url)
            .build()

        val response = client.newCall(request).execute()
        val responseBody = response.body?.string()

        val postboxHistory = mutableListOf<PostboxHistoryItem>()

        if (response.isSuccessful && !responseBody.isNullOrBlank()) {
            val jsonArray = JSONArray(responseBody)
            for (i in 0 until jsonArray.length()) {
                val jsonObject = jsonArray.getJSONObject(i)
                val date = jsonObject.getString("Date")
                val userName = jsonObject.getString("UserName")
                val postboxId = jsonObject.getString("PostboxId")
                val type = jsonObject.optString("Type", null.toString())
                val success = jsonObject.getBoolean("Success")

                val dateFormatter =
                    SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault())
                val parsedDate = dateFormatter.parse(date)

                if (parsedDate != null) {
                    val historyItem =
                        PostboxHistoryItem(parsedDate, userName, postboxId, type, success)
                    postboxHistory.add(historyItem)
                }
            }
        }

        return postboxHistory
    }
}
