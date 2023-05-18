package com.um.feri.direct4me.android.model

import jakarta.validation.constraints.NotNull
import java.util.*

data class User(
    @field:NotNull val guid: String,
    @field:NotNull val email: String,
    @field:NotNull val password: String,
    @field:NotNull val fullname: String,
    @field:NotNull val totalLogins: Int,
    @field:NotNull val lastAccess: Date
)