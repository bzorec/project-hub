package com.um.feri.direct4me.android

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST
import com.um.feri.direct4me.android.model.OpenBoxRequest
import com.um.feri.direct4me.android.model.TokenResponse

interface Direct4meService {
    @POST("openBox")
    fun openBox(
        @Body request: OpenBoxRequest
    ): Call<TokenResponse>
}
