/**
 * @typedef {Object} ChatMessage
 * @property {number} id
 * @property {number} senderId
 * @property {number} chatGroupId
 * @property {string} content
 * @property {string} sentAt
 */

/**
 * @typedef {Object} ChatGroup
 * @property {number} id
 * @property {string} name
 * @property {string} description
 * @property {string} createdAt
 */

/**
 * @typedef {Object} User
 * @property {number} id
 * @property {string} username
 * @property {string} email
 */

/**
 * @typedef {Object} ChatState
 * @property {ChatMessage[]} messages
 * @property {ChatGroup[]} groups
 * @property {User} currentUser
 * @property {number} selectedGroupId
 * @property {string} newMessage
 * @property {() => Promise<void>} loadGroups
 * @property {(groupId: number) => Promise<void>} loadMessages
 * @property {() => Promise<void>} sendMessage
 */