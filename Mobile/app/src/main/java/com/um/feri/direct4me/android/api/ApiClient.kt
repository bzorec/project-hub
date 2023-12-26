package com.um.feri.direct4me.android.api

import com.um.feri.direct4me.android.BuildConfig
import android.content.Context
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.JsonArrayRequest
import com.android.volley.toolbox.JsonObjectRequest
import com.android.volley.toolbox.Volley
import com.google.gson.Gson
import com.um.feri.direct4me.android.api.model.faceUnlock.FaceUnlockRequest
import com.um.feri.direct4me.android.api.model.faceUnlock.FaceUnlockResponse
import com.um.feri.direct4me.android.api.model.login.LoginRequest
import com.um.feri.direct4me.android.api.model.login.LoginResponse
import com.um.feri.direct4me.android.ui.history.PostboxHistoryItem
import org.json.JSONObject
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

        val jsonArrayRequest =
            JsonArrayRequest(Request.Method.GET, url, null, Response.Listener { response ->
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
                                date, userName, postboxId, type, success
                            )
                        )
                    }
                }
                callback(postboxHistoryItems)
            }) { error ->
                error.printStackTrace()
                callback(emptyList())
            }
        queue.add(jsonArrayRequest)
    }

    fun getUserHistory(userGuid: String, callback: (List<PostboxHistoryItem>) -> Unit) {
        val queue = Volley.newRequestQueue(context)
        val url = "${BuildConfig.API_BASE_URL}postboxes/history/user/$userGuid"

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
                                date, userName, postboxId, type, success
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

    fun faceUnlock(
        email: String,
        imageBytes: ByteArray,
        callback: (FaceUnlockResponse?) -> Unit
    ) {
        val queue = Volley.newRequestQueue(context)
        val url = "${BuildConfig.API_BASE_URL}users/login/face-login"

        val encoder = Base64.getEncoder();
        val base64Image = encoder.encodeToString(imageBytes)

        val faceUnlockRequest = FaceUnlockRequest(email, base64Image)
        val requestData = JSONObject(Gson().toJson(faceUnlockRequest))

        val jsonObjectRequest = JsonObjectRequest(Request.Method.POST, url, requestData,
            { response ->
                val authResponse =
                    Gson().fromJson(response.toString(), FaceUnlockResponse::class.java)
                callback(authResponse)
            },
            { error ->
                error.printStackTrace()
                callback(null)
            })

        queue.add(jsonObjectRequest)
    }

    fun login(
        email: String,
        password: String,
        callback: (LoginResponse?) -> Unit
    ) {
        val queue = Volley.newRequestQueue(context)
        val url = "${BuildConfig.API_BASE_URL}users/login"

        val loginRequest = LoginRequest(email, password)
        val requestData = JSONObject(Gson().toJson(loginRequest))

        val jsonObjectRequest = JsonObjectRequest(Request.Method.POST, url, requestData,
            { response ->
                val loginResponse = Gson().fromJson(response.toString(), LoginResponse::class.java)
                callback(loginResponse)
            },
            { error ->
                error.printStackTrace()
                callback(null)
            })

        queue.add(jsonObjectRequest)
    }
}
