package com.um.feri.direct4me.android.model

import com.google.gson.annotations.SerializedName

data class OpenBoxRequest(
    @SerializedName("deliveryId") val deliveryId: Int,
    @SerializedName("boxId") val boxId: Int,
    @SerializedName("tokenFormat") val tokenFormat: Int,
    @SerializedName("latitude") val latitude: Int,
    @SerializedName("longitude") val longitude: Int,
    @SerializedName("qrCodeInfo") val qrCodeInfo: String,
    @SerializedName("terminalSeed") val terminalSeed: Int,
    @SerializedName("isMultibox") val isMultibox: Boolean,
    @SerializedName("doorIndex") val doorIndex: Int,
    @SerializedName("addAccessLog") val addAccessLog: Boolean
)
