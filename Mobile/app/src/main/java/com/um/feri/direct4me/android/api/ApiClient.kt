package com.um.feri.direct4me.android.api

import com.um.feri.direct4me.android.BuildConfig
import android.content.Context
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonArrayRequest
import com.android.volley.toolbox.Volley
import com.um.feri.direct4me.android.ui.history.PostboxHistoryItem
import java.text.ParseException
import java.text.SimpleDateFormat
import java.util.*

class ApiClient(private val context: Context) {

    private val dateFormatter =
        SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.getDefault()).apply {
            timeZone = TimeZone.getTimeZone("UTC")
        }

    fun getPostboxHistory(boxGuid: String, callback: (List<PostboxHistoryItem>) -> Unit) {
        val queue = Volley.newRequestQueue(context)
        val url = "${BuildConfig.API_BASE_URL}postboxes/history/box/$boxGuid"

        val jsonArrayRequest = JsonArrayRequest(Request.Method.GET, url, null,
            Response.Listener { response ->
                val postboxHistoryItems = mutableListOf<PostboxHistoryItem>()
                for (i in 0 until response.length()) {
                    val item = response.getJSONObject(i)
                    val dateStr = item.optString("date", null) // Use null for the default value
                    var date: Date? = null
                    try {
                        if (dateStr != null) {
                            date = dateFormatter.parse(dateStr)
                        }
                    } catch (e: ParseException) {
                        e.printStackTrace()
                        // Handle the parsing error or continue the loop
                    }

                    if (date != null) {
                        val userName = item.getString("userName")
                        val postboxId = item.getString("postboxId")
                        val type = item.optString("type")
                        val success = item.getBoolean("success")
                        postboxHistoryItems.add(
                            PostboxHistoryItem(
                                date,
                                userName,
                                postboxId,
                                type,
                                success
                            )
                        )
                    }
                }
                callback(postboxHistoryItems)
            }
        ) { error ->
            error.printStackTrace()
            callback(emptyList())
        }
        queue.add(jsonArrayRequest)
    }

    fun getUserHistory(userGuid: String, callback: (List<PostboxHistoryItem>) -> Unit) {
        val queue = Volley.newRequestQueue(context)
        val url = "${BuildConfig.API_BASE_URL}/postboxes/history/user/$userGuid"

        val jsonArrayRequest = JsonArrayRequest(
            Request.Method.GET, url, null,
            { response ->
                val userHistoryItems = mutableListOf<PostboxHistoryItem>()
                for (i in 0 until response.length()) {
                    val item = response.getJSONObject(i)
                    val dateStr = item.optString("date", null) // Use null for the default value
                    var date: Date? = null
                    try {
                        if (dateStr != null) {
                            date = dateFormatter.parse(dateStr)
                        }
                    } catch (e: ParseException) {
                        e.printStackTrace()
                        // Handle the parsing error or continue the loop
                    }

                    if (date != null) {
                        val userName = item.getString("userName")
                        val postboxId = item.getString("postboxId")
                        val type = item.optString("type")
                        val success = item.getBoolean("success")
                        userHistoryItems.add(
                            PostboxHistoryItem(
                                date,
                                userName,
                                postboxId,
                                type,
                                success
                            )
                        )
                    }
                }
                callback(userHistoryItems)
            },
        ) { error ->
            error.printStackTrace()
            callback(emptyList()) // Or handle the error appropriately
        }

        queue.add(jsonArrayRequest)
    }
}
