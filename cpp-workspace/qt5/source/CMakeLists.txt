cmake_minimum_required(VERSION 3.21)

file(GLOB HEADERS CONFIGURE_DEPENDS "${INCLUDE_DIR}/*.h")
file(GLOB SRCS    CONFIGURE_DEPENDS "*.cpp")
file(GLOB UI      CONFIGURE_DEPENDS "ui/*.ui")

add_executable(
	${PROJECT_NAME}
		${SRCS}
		${HEADERS}
		${UI}
)

target_link_libraries(
	${PROJECT_NAME}
	PRIVATE
		Qt5::Widgets
)

target_compile_definitions(
	${PROJECT_NAME}
	PRIVATE
		PROJECT_NAME="${PROJECT_NAME}"
		PROJECT_VERSION="${PROJECT_VERSION}"
)

set(QT5_LUPDATE_OPTIONS -no-obsolete)

set(TS_FILES)
foreach(lang IN LISTS QT5_I18N_TRANSLATED_LANGUAGES)
	list(APPEND TS_FILES ${CMAKE_CURRENT_SOURCE_DIR}/translations/${PROJECT_NAME}_${lang}.ts)
endforeach()
qt5_create_translation(
	QM_FILES
		${HEADERS} ${SRCS} ${UI}
		${TS_FILES}
	OPTIONS ${QT5_LUPDATE_OPTIONS}
)

if (WIN32 AND DISABLE_CONSOLE)
	# Disables console window on Windows
	set_target_properties(${PROJECT_NAME} PROPERTIES WIN32_EXECUTABLE ON)
endif()


# --- install rules --- #
if (WIN32)
	install(TARGETS ${PROJECT_NAME}
		BUNDLE  DESTINATION .
		RUNTIME DESTINATION .
	)
else()
	install(TARGETS ${PROJECT_NAME}
		BUNDLE  DESTINATION .
		RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
	)
endif()

