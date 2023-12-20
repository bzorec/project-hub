package com.um.feri.direct4me.android.ui.history

import java.util.*


data class PostboxHistoryItem(
    val date: Date?,
    val userName: String,
    val postboxId: String,
    val type: String?,
    val success: Boolean
)
