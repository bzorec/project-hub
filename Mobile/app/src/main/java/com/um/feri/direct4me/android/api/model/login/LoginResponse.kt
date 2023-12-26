package com.um.feri.direct4me.android.api.model.login

import java.util.Date

data class LoginResponse(
    val guid: String,
    val email: String,
    val fullname: String,
    val totalLogins: Int,
    val lastAccess: Date,
    val isFaceUnlockEnabled: Boolean
)
