package com.um.feri.direct4me.android.ui.history

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.um.feri.direct4me.android.api.ApiClient
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.util.Date


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
                    println(e.message)
                    // Mock response for postboxId 542
                    val mockData = listOf(
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "542", "QR", true
                        ),
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "542", "QR", true
                        ),
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "123", "QR", false
                        ),
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "123", "QR", false
                        )
                    )
                    userHistory.value = mockData
                }
            }
        }
    }
}
