package com.um.feri.direct4me.android.ui.login

import android.content.Context
import android.content.SharedPreferences
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.google.gson.Gson
import com.um.feri.direct4me.android.api.ApiClient
import java.util.Date

class LoginViewModel : ViewModel() {
    private val userModelKey = "userModel"
    private lateinit var sharedPreferences: SharedPreferences
    val loginSuccess = MutableLiveData<Boolean>()
    val takePictureEvent = MutableLiveData<Boolean>()

    fun basicLogin(context: Context, email: String, password: String) {
        ApiClient(context).login(email, password) { response ->
            if (response != null) {
                val userModel = UserSimple(
                    email = response.email,
                    fullname = response.fullname,
                    isFaceUnlockEnabled = response.isFaceUnlockEnabled,
                    guid = response.guid,
                    totalLogins = response.totalLogins,
                    lastAccess = response.lastAccess
                )
                saveUserModel(context, userModel)
                loginSuccess.postValue(true)
            } else {
                loginSuccess.postValue(false)
            }
        }
    }

    fun faceUnlock(context: Context, email: String, imageBytes: ByteArray) {
        ApiClient(context).faceUnlock(email, imageBytes) { response ->
            if (response != null) {
                val userModel = UserSimple(
                    email = response.email,
                    fullname = response.fullname,
                    isFaceUnlockEnabled = true,
                    guid = response.guid,
                    totalLogins = response.totalLogins,
                    lastAccess = response.lastAccess
                )
                saveUserModel(context, userModel)
                loginSuccess.postValue(true)
            } else {
                loginSuccess.postValue(false)
            }
        }
    }

    fun initSharedPreferences(context: Context) {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)
    }

    private fun saveUserModel(context: Context, userModel: UserSimple) {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        val userModelJson = Gson().toJson(userModel)
        editor.putString(userModelKey, userModelJson)
        editor.apply()
    }

    fun getUserModel(context: Context): UserSimple? {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)
        val userModelJson = sharedPreferences.getString(userModelKey, null) ?: return null
        return Gson().fromJson(userModelJson, UserSimple::class.java)
    }

    fun removeUserModel(context: Context) {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.remove(userModelKey)
        editor.apply()
    }
}


data class UserSimple(
    val guid: String,
    val email: String,
    val fullname: String,
    val totalLogins: Int,
    val lastAccess: Date,
    val isFaceUnlockEnabled: Boolean
)