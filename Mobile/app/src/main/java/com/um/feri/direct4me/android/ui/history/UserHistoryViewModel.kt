package com.um.feri.direct4me.android.ui.history

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.um.feri.direct4me.android.api.ApiClient
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext


class UserHistoryViewModel : ViewModel() {
    val userHistory: MutableLiveData<List<PostboxHistoryItem>> = MutableLiveData()

    @OptIn(DelicateCoroutinesApi::class)
    fun fetchUserHistory(userGuid: String) {

        GlobalScope.launch {
            try {
                val userHistoryData = ApiClient.getUserHistory(userGuid)
                withContext(Dispatchers.Main) {
                    userHistory.value = userHistoryData
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                }
            }
        }
    }
}
