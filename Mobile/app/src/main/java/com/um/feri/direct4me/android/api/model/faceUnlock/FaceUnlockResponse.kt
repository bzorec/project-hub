package com.um.feri.direct4me.android.api.model.faceUnlock

import java.util.Date

data class FaceUnlockResponse(
    val guid: String,
    val email: String,
    val fullname: String,
    val totalLogins: Int,
    val lastAccess: Date
)
