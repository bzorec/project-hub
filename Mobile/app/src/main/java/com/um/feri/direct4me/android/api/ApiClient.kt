package com.um.feri.direct4me.android.api

import com.google.gson.GsonBuilder
import com.google.gson.JsonDeserializationContext
import com.google.gson.JsonDeserializer
import com.google.gson.JsonElement
import com.google.gson.reflect.TypeToken
import com.um.feri.direct4me.android.ui.history.PostboxHistoryItem
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET
import retrofit2.http.Path
import java.lang.reflect.Type
import java.text.SimpleDateFormat
import java.util.*
import java.util.concurrent.TimeUnit

object ApiClient {
    private val client = OkHttpClient.Builder()
        .connectTimeout(1, TimeUnit.SECONDS) // Set connection timeout to 10 seconds
        .readTimeout(1, TimeUnit.SECONDS) // Set read timeout to 10 seconds
        .build()

    private const val BASE_URL = "http://192.168.10.207:5000"

    private val gson = GsonBuilder()
        .registerTypeAdapter(
            object : TypeToken<List<PostboxHistoryItem>>() {}.type,
            PostboxHistoryConverter()
        )
        .create()

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(BASE_URL)
        .client(client)
        .addConverterFactory(GsonConverterFactory.create(gson))
        .build()

    private val apiService: ApiService = retrofit.create(ApiService::class.java)

    fun getUserHistory(userGuid: String): List<PostboxHistoryItem> {
        val call = apiService.getUserHistory(userGuid)
        val response = call.execute()
        return response.body() ?: emptyList()
    }

    fun getPostboxHistory(boxGuid: String): List<PostboxHistoryItem> {
        val call = apiService.getPostboxHistory(boxGuid)
        val response = call.execute()
        return response.body() ?: emptyList()
    }

    interface ApiService {
        @GET("postboxes/history/user/{userGuid}")
        fun getUserHistory(@Path("userGuid") userGuid: String): Call<List<PostboxHistoryItem>>

        @GET("postboxes/history/box/{boxGuid}")
        fun getPostboxHistory(@Path("boxGuid") boxGuid: String): Call<List<PostboxHistoryItem>>
    }
}

class PostboxHistoryConverter : JsonDeserializer<List<PostboxHistoryItem>> {
    private val dateFormatter = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault())

    override fun deserialize(
        json: JsonElement?,
        typeOfT: Type?,
        context: JsonDeserializationContext?
    ): List<PostboxHistoryItem> {
        val jsonArray = json?.asJsonArray ?: return emptyList()
        val historyItems = mutableListOf<PostboxHistoryItem>()

        for (jsonElement in jsonArray) {
            val jsonObject = jsonElement.asJsonObject

            val date = jsonObject.get("Date")?.asString ?: ""
            val userName = jsonObject.get("UserName")?.asString ?: ""
            val postboxId = jsonObject.get("PostboxId")?.asString ?: ""
            val type = jsonObject.get("Type")?.asString
            val success = jsonObject.get("Success")?.asBoolean ?: false

            val parsedDate = dateFormatter.parse(date)

            if (parsedDate != null) {
                val historyItem = PostboxHistoryItem(parsedDate, userName, postboxId, type, success)
                historyItems.add(historyItem)
            }
        }

        return historyItems
    }
}