package com.um.feri.direct4me.android.core

import android.content.Context
import android.content.SharedPreferences
import com.google.gson.Gson
import com.um.feri.direct4me.android.ui.login.UserSimple

class PreferencesHandler private constructor(context: Context) {
    private val sharedPreferences: SharedPreferences =
        context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)

    companion object {
        private lateinit var instance: PreferencesHandler
        private const val userModelKey = "userModel"

        fun init(context: Context): PreferencesHandler {
            if (!::instance.isInitialized) {
                instance = PreferencesHandler(context)
            }
            return instance
        }

        fun getInstance(): PreferencesHandler {
            if (!::instance.isInitialized) {
                throw IllegalStateException("PreferencesHandler is not initialized")
            }
            return instance
        }
    }

    fun saveUserModel(userModel: UserSimple) {
        val userModelJson = Gson().toJson(userModel)
        sharedPreferences.edit().putString(userModelKey, userModelJson).apply()
    }

    fun getUserModel(): UserSimple? {
        val userModelJson = sharedPreferences.getString(userModelKey, null) ?: return null
        return try {
            Gson().fromJson(userModelJson, UserSimple::class.java)
        } catch (e: Exception) {
            null
        }
    }

    fun removeUserModel() {
        sharedPreferences.edit().remove(userModelKey).apply()
    }
}
