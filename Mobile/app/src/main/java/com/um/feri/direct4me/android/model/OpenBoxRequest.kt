package com.um.feri.direct4me.android.model

import com.google.gson.annotations.SerializedName

data class OpenBoxRequest(
    var boxId: Int,
    var deliveryId: Int = 0,
    var tokenFormat: Int = 5,
    var isMultibox: Boolean = false,
    var addAccessLog: Boolean = true
)
