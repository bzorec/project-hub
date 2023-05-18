package com.um.feri.direct4me.android.model

import com.google.gson.annotations.SerializedName

data class OpenBoxRequest(
    val boxId: String,
    val tokenFormat: Int
)
